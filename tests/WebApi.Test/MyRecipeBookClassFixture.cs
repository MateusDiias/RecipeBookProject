using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;

namespace WebApi.Test
{
    // ao invez das classes de teste implementarem o IClassFixture, eu faço essa classe custom implementar. Essa classe conterá métodos
    // que visam diminuir a duplicação de código nos testes
    public class MyRecipeBookClassFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public MyRecipeBookClassFixture(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        protected async Task<HttpResponseMessage> DoPost(string method, object request, string culture = "en")
        {
            ChangeRequestCulture(culture);

            return await _httpClient.PostAsJsonAsync(method, request);
        }

        private void ChangeRequestCulture(string culture)
        {
            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
        }
    }
}
