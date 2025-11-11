using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infraestructure.DataAccess;
using MyRecipeBook.Infraestructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Cryptography;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infrastructure.Services.LoggedUser;

namespace MyRecipeBook.Infraestructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddPasswordEncripter(services, configuration);
            AddRepositories(services);
            AddTokens(services, configuration);
            AddLoggedUser(services);


            if (configuration.isUnitTestEnvoriment()) return;
            AddDbContext_SqlServer(services, configuration);
            AddFluentMigrator_SqlServer(services, configuration);
        }

        private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
        }
        
        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        }

        private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All();
            });
        }

        public static void AddTokens(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");


            services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
            services.AddScoped<IAccesTokenValidator>(option => new JwtTokenValidator(signingKey!));
        }

        public static void AddLoggedUser(IServiceCollection services)
        {
            services.AddScoped<ILoggedUser, LoggedUser>();
        }

        private static void AddPasswordEncripter(IServiceCollection services, IConfiguration configuration)
        {
            //var additionalKey = configuration.GetSection("Settings:Password:AdditionalKey").Value;
            //o .Value do método acima sempre vai retornar String, portanto não é flexível. melhoria:
            var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");

            services.AddScoped<IPasswordEncripter>(option => new Sha512Encripter(additionalKey!));
        }
    }
}
