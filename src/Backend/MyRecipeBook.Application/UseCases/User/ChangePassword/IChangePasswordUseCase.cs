using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword
{
    public interface IChangePasswordUseCase
    {
        public Task Execute(RequestChangePasswordJson request);
    }
}
