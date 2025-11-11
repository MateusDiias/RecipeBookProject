using System.Runtime.InteropServices;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Update
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserUseCase(ILoggedUser loggedUser, IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork, IUserUpdateOnlyRepository userUpdateOnlyRepository)
        {
            _loggedUser = loggedUser;
            _userReadOnlyRepository = userReadOnlyRepository;
            _unitOfWork = unitOfWork;
            _userUpdateOnlyRepository = userUpdateOnlyRepository;
        }

        public async Task Execute(RequestUpdateUserJson request)
        {
            var loggedUser = await _loggedUser.User();

            await Validate(request, loggedUser.Email);

            var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);

            user.Name = request.Name;
            user.Email = request.Email;

            _userUpdateOnlyRepository.Update(user);

            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestUpdateUserJson request, string currentEmail)
        {
            var validator = new UpdateUserValidator();

            var result = validator.Validate(request);

            if (result.IsValid && !currentEmail.Equals(request.Email)) 
            {
                var userExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
                if (userExist)
                {
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
                }
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
