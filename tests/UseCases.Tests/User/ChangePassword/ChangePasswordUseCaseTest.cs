using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.User.ChangePassword
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = password;

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(request); };

            await act.ShouldNotThrowAsync();

            var passwordEncripter = PasswordEncripterBuilder.Build();

            user.Password.ShouldBe(passwordEncripter.Encrypt(request.NewPassword));
        }
        
        [Fact]
        public async Task Error_NewPassword_Empty()
        {
            (var user, var password) = UserBuilder.Build();

            var request = new RequestChangePasswordJson
            {
                Password = password,
                NewPassword = string.Empty
            };

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(request); };

            var error = await act.ShouldThrowAsync<ErrorOnValidationException>();
            error.ErrorMessages.ShouldSatisfyAllConditions(
            errors => errors.Count.ShouldBe(1),
            error => error.ShouldContain(ResourceMessagesException.PASSWORD_EMPTY));

            var passwordEncripter = PasswordEncripterBuilder.Build();

            user.Password.ShouldBe(passwordEncripter.Encrypt(password));
        }
        
        [Fact]
        public async Task Error_CurrentPassword_Different()
        {
            (var user, var password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(request); };

            var error = await act.ShouldThrowAsync<ErrorOnValidationException>();
            error.ErrorMessages.ShouldSatisfyAllConditions(
            errors => errors.Count.ShouldBe(1),
            error => error.ShouldContain(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

            var passwordEncripter = PasswordEncripterBuilder.Build();

            user.Password.ShouldBe(passwordEncripter.Encrypt(password));
        }

        private static ChangePasswordUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var unitWork = UnitOfWorkBuilder.Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();

            return new ChangePasswordUseCase(loggedUser, repository, unitWork, passwordEncripter);
        }
    }
}
