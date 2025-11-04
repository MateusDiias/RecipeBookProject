using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Filters;

namespace MyRecipeBook.API.Attributes
{
    // Atributo de autorização, verificar se o token e usuario é valido. a classe AuthenticatedUserFilter que implementa o atributo. 102
    public class AuthenticatedUserAttribute : TypeFilterAttribute
    {
        public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
        {
        }
    }
}
