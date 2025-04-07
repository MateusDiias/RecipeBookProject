using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using MyRecipeBook.Exceptions;
using Shouldly;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register
{
    // Teste de integração. para sinalizar esse tipo de teste, deve herdar da classe IClassFixture, passando um servidor T, o .net disponibiliza um servidor 
    public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly string method = "user";
        private readonly HttpClient _httpClient;

        public RegisterUserTest(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var response = await _httpClient.PostAsJsonAsync(method, request);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            // fazer parse da response em um objeto dinamico, e verificar se nele tem as propriedades precisas
            // ler a response como stream, boa pratica
            await using var responseBody = await response.Content.ReadAsStreamAsync();
            // fazendo parse em um documento, não em uma classe
            var responseData = await JsonDocument.ParseAsync(responseBody);

            // acessa o doc, esse doc tem um propriedade com o nome name, pega como str o valor dela, e testa
            responseData.RootElement.GetProperty("name").GetString().ShouldSatisfyAllConditions(
                name => name.ShouldNotBeNullOrWhiteSpace(),
                name => name.ShouldBe(request.Name));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

            var response = await _httpClient.PostAsJsonAsync(method, request);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY",new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(
                e => e.ShouldHaveSingleItem(),
                e => e.ShouldContain(e => e.GetString()!.Equals(expectedMessage)));
        }
    }
}
