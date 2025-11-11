using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.User.Update
{
    public class UpdateUserUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJson.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(request); };

            await act.ShouldNotThrowAsync();

            user.Name.ShouldBe(request.Name);
            user.Email.ShouldBe(request.Email);
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJson.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(request); };

            var error = await act.ShouldThrowAsync<ErrorOnValidationException>();
            error.ErrorMessages.ShouldSatisfyAllConditions(
            errors => errors.Count.ShouldBe(1),
            error => error.ShouldContain(ResourceMessagesException.NAME_EMPTY));

            user.Name.ShouldNotBe(request.Name);
            user.Email.ShouldNotBe(request.Email);
        }
        
        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJson.Build();

            var useCase = CreateUseCase(user, request.Email);

            Func<Task> act = async () => { await useCase.Execute(request); };

            var error = await act.ShouldThrowAsync<ErrorOnValidationException>();
            error.ErrorMessages.ShouldSatisfyAllConditions(
            errors => errors.Count.ShouldBe(1),
            error => error.ShouldContain(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

            user.Name.ShouldNotBe(request.Name);
            user.Email.ShouldNotBe(request.Email);
        }

        public UpdateUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, string? email = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var updateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            var readRepository = new UserReadOnlyRepositoryBuilder();
            if (!string.IsNullOrEmpty(email))
            {
                readRepository.ExistAciveUserWithEmail(email);
            }

            return new UpdateUserUseCase(loggedUser, readRepository.Build(), unitOfWork, updateRepository);

        }
    }
}
