using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Infraestructure.DataAccess;

namespace MyRecipeBook.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitWork
    {
        private readonly MyRecipeBookDbContext _dbContext;

        public UnitOfWork(MyRecipeBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
