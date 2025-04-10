using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using Shouldly;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Login.DoLogin
{
    public class DoLoginTest : MyRecipeBookClassFixture
    {
        private readonly string method = "login";

        private readonly string _email;
        private readonly string _password;
        private readonly string _name;
        

        public DoLoginTest(CustomWebApplicationFactory factory) : base(factory) 
        {
            _email = factory.getEmail();
            _password = factory.getPassword();
            _name = factory.getName();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            var response = await DoPost(method, request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            
            responseData.RootElement.GetProperty("name").GetString().ShouldSatisfyAllConditions(
                name => name.ShouldNotBeNullOrWhiteSpace(),
                name => name.ShouldBe(_name));

        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Login_Invalid(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();

            var response = await DoPost(method, request, culture);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(
                e => e.ShouldHaveSingleItem(),
                e => e.ShouldContain(e => e.GetString()!.Equals(expectedMessage)));
        }
    }
}
