using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infraestructure.DataAccess;

namespace MyRecipeBook.Infrastructure.Services.LoggedUser
{
    //103
    public class LoggedUser : ILoggedUser
    {
        private readonly MyRecipeBookDbContext _dbContext;
        private readonly ITokenProvider _tokenProvider;

        public LoggedUser(MyRecipeBookDbContext dbContrext, ITokenProvider tokenProvider)
        {
            _dbContext = dbContrext;   
            _tokenProvider = tokenProvider;
        }
        public async Task<User> User()
        {
            var token = _tokenProvider.Value();

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var identifier = Guid.Parse(jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value);

            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstAsync(user => user.Active && user.UserIdentifier == identifier);
        }
    }
}
