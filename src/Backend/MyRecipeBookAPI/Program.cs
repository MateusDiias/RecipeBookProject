using MyRecipeBookAPI.Filters;
using MyRecipeBookAPI.Middleware;
using MyRecipeBook.Application;
using MyRecipeBook.Infraestructure;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Infrastructure.Migrations;
using MyRecipeBook.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

app.Run();

void MigrateDatabase()
{
    if (builder.Configuration.isUnitTestEnvoriment()) return;
    var serviceScope = app.Services.GetRequiredService<IServiceProvider>().CreateScope();
    DatabaseMigration.Migrate(builder.Configuration.ConnectionString(), serviceScope.ServiceProvider);
}

public partial class Program
{

}