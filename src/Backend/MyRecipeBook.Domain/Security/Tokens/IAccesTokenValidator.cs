namespace MyRecipeBook.Domain.Security.Tokens
{
    public interface IAccesTokenValidator
    {
        public Guid ValidateAndGetUserIdentifier(string token);
    }
}
