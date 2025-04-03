using Microsoft.Extensions.Configuration;


namespace MyRecipeBook.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {   
        public static bool isUnitTestEnvoriment(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("InMemoryTest");
        }

        public static string ConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("ConnectionSqlServer")!;
        }
    }
}
