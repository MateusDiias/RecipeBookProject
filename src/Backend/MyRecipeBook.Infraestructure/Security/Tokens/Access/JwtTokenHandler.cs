using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access
{
    public abstract class JwtTokenHandler
    {
        protected SymmetricSecurityKey SecurityKey(string signinKey)
        {
            var bytes = Encoding.UTF8.GetBytes(signinKey);

            return new SymmetricSecurityKey(bytes);
        }
    }
}
