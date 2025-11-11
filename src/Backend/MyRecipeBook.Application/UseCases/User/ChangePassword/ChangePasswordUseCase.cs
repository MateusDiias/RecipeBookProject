using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserUpdateOnlyRepository _updateRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordEncripter _passwordEncripter;

        public ChangePasswordUseCase(ILoggedUser loggedUser, IUserUpdateOnlyRepository updateRepository, IUnitOfWork unitOfWork, IPasswordEncripter passwordEncripter)
        {
            _loggedUser = loggedUser;  
            _updateRepository = updateRepository;
            _unitOfWork = unitOfWork;
            _passwordEncripter = passwordEncripter;
        }

        public async Task Execute(RequestChangePasswordJson request)
        {
            var loggedUser = await _loggedUser.User();

            Validate(request, loggedUser);

            var user = await _updateRepository.GetById(loggedUser.Id);

            user.Password = _passwordEncripter.Encrypt(request.NewPassword);

            _updateRepository.Update(user);

            await _unitOfWork.Commit();
        }

        private void Validate(RequestChangePasswordJson request, Domain.Entities.User user)
        {
            var result = new ChangePasswordValidator().Validate(request);

            var currentPassword = _passwordEncripter.Encrypt(request.Password);

            if (currentPassword != user.Password)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
            }

            if(!result.IsValid)
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}
