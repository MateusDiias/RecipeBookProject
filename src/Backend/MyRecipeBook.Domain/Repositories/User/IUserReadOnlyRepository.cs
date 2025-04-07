namespace MyRecipeBook.Domain.Repositories.User
{
    public interface IUserReadOnlyRepository
    {
        public Task<bool> ExistAciveUserWithEmail(string email);

        public Task<Entities.User?> GetByEmailAndPassword(string email, string password);
    }
}
