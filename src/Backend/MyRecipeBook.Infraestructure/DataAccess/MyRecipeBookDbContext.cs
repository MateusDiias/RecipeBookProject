using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infraestructure.DataAccess
{
    // classe para configurar o Entity Framework, usado para manipular as entidades no banco
    public class MyRecipeBookDbContext : DbContext
    {
        public MyRecipeBookDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyRecipeBookDbContext).Assembly);
        }
    }
}
