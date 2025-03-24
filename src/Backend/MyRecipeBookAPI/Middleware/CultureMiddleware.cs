using System.Globalization;
using System.Runtime.Versioning;

namespace MyRecipeBookAPI.Middleware
{
    // Middleware, usado para interceptar uma requisição antes de chegar ao controller.
    // Essa middleware será usado para alterar a "Cultura" da aplicação/identificar o idioma e altarar.
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //Toda middleware em ASP.NET deve conter este método abaixo:
        public async Task Invoke(HttpContext context)
        {
            var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures);

            // consultar a linguagem que a requisição enviou no Header http.
            var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            // Passando para a API a nova linguagem
            var cultureInfo = new CultureInfo("en");

            if (!string.IsNullOrWhiteSpace(requestedCulture) && supportedLanguages.Any(c => c.Name.Equals(requestedCulture))) 
            {
                cultureInfo = new CultureInfo(requestedCulture);
            }

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            await _next(context);
        }
    }
}
