using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestUpdateUserJson
    {
        public static MyRecipeBook.Communication.Requests.RequestUpdateUserJson Build()
        {
            return new Faker<MyRecipeBook.Communication.Requests.RequestUpdateUserJson>()
                .RuleFor(user => user.Name, f => f.Person.FirstName)
                .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name));
        }
    }
}
