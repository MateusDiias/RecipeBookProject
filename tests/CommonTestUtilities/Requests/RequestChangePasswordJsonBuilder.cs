using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestChangePasswordJsonBuilder
    {
        public static RequestChangePasswordJson Build(int passwordLength = 10)
        {
            return new Faker<RequestChangePasswordJson>()
                .RuleFor(p => p.Password, f => f.Internet.Password())
                .RuleFor(p => p.NewPassword, f => f.Internet.Password(passwordLength));
        }
    }
}
