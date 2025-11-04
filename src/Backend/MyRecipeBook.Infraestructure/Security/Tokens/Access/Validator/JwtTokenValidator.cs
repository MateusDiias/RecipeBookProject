using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator
{
    // funçao para validar se o token é valido, se esta inspirado, e devolve o identifier do user. 101
    internal class JwtTokenValidator : JwtTokenHandler, IAccesTokenValidator
    {
        private readonly string _signinKey;

        public JwtTokenValidator(string signinKey)
        {
            _signinKey = signinKey;
        }

        public Guid ValidateAndGetUserIdentifier(string token)
        {
            var validationParameter = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = SecurityKey(_signinKey),
                ClockSkew = new TimeSpan(0)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, validationParameter, out _);

            var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            return Guid.Parse(userIdentifier);
        }
    }
}
