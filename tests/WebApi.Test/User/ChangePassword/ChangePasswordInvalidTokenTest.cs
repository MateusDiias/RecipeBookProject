using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;

namespace WebApi.Test.User.ChangePassword
{
    public class ChangePasswordInvalidTokenTest : MyRecipeBookClassFixture
    {
        private readonly string METHOD = "user/change-password";
        public ChangePasswordInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            var request = RequestChangePasswordJsonBuilder.Build();

            var response = await DoPut(METHOD, request, token: "token invalido");

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Without_Token()
        {
            var request = RequestChangePasswordJsonBuilder.Build();

            var response = await DoPut(METHOD, request, token: string.Empty);

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_User_NotFound()
        {
            var request = RequestChangePasswordJsonBuilder.Build();

            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoPut(METHOD, request, token: token);

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
