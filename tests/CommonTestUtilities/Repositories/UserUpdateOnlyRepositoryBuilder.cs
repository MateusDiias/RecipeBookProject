using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories
{
    public class UserUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<IUserUpdateOnlyRepository> _repository;

        public UserUpdateOnlyRepositoryBuilder()
        {
            _repository = new Mock<IUserUpdateOnlyRepository>();
        }

        public UserUpdateOnlyRepositoryBuilder GetById(User user)
        {
            _repository.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);
            return this;
        }

        public void Update(User user)
        {
            _repository.Setup(repository => repository.Update(user));
        }

        public IUserUpdateOnlyRepository Build()
        {
            return _repository.Object;
        }
    }
}
