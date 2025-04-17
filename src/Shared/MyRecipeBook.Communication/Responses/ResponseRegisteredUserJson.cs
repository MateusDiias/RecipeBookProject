namespace MyRecipeBook.Communication.Responses
{
    public class ResponseRegisteredUserJson
    {
        public string Name { get; set; } = string.Empty;
        public ReponseTokensJson Tokens { get; set; } = default!;
    }
}
