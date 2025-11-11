using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using Shouldly;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update
{
    public class UpdateUserTest : MyRecipeBookClassFixture
    {
        private readonly string METHOD = "user";

        private readonly Guid _userIdentifier;
        public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestUpdateUserJson.Build();

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPut(METHOD, request, token);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            var request = RequestUpdateUserJson.Build();
            request.Name = string.Empty;

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPut(METHOD, request, token, culture);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(
                e => e.ShouldHaveSingleItem(),
                e => e.ShouldContain(e => e.GetString()!.Equals(expectedMessage)));
        }

    }
}
