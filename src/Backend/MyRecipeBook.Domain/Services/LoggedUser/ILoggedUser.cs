using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Services.LoggedUser
{
    public interface ILoggedUser
    {
        public Task<User> User();
    }
}
