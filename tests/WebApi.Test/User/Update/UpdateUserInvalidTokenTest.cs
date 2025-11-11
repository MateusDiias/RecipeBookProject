using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;

namespace WebApi.Test.User.Update
{
    public class UpdateUserInvalidTokenTest : MyRecipeBookClassFixture
    {
        private readonly string METHOD = "user";
        public UpdateUserInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            var request = RequestUpdateUserJson.Build();

            var response = await DoPut(METHOD, request, token: "token invalido");

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Without_Token()
        {
            var request = RequestUpdateUserJson.Build();

            var response = await DoPut(METHOD, request, token: string.Empty);

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_User_NotFound()
        {
            var request = RequestUpdateUserJson.Build();

            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoPut(METHOD, request, token: token);

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
