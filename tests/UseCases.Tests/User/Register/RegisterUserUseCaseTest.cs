using System.Text;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase();
            
            var result = await useCase.Execute(request);

            result.ShouldNotBeNull(); 
            result.Name.ShouldBe(request.Name);
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase(request.Email);
            
            // armazenando dentro da variável act, uma função. não executa a função, apenas armazena na variável.
            Func<Task> act = async () => await useCase.Execute(request);

            // sprint 1. aula 71
            var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();
            exception.ErrorMessages.ShouldHaveSingleItem()
                .ShouldBe(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);
        }
        
        [Fact]
        public async Task Error_Name_Empty()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);

            // sprint 1. aula 71
            var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();
            exception.ErrorMessages.ShouldHaveSingleItem()
                .ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        private RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var mapper = MapperBuilder.Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

            if(!string.IsNullOrEmpty(email))
                readRepositoryBuilder.ExistAciveUserWithEmail(email);

            return new RegisterUserUseCase(readRepositoryBuilder.Build(), writeRepository, mapper, passwordEncripter, unitOfWork);
        }
    }
}
