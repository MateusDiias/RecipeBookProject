using AutoMapper;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    // Classe contendo as regras de negócio para o registro de um usuário.
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserReadOnlyRepository _readOnlyRepository;
        private readonly IUserWriteOnlyRepository _writeOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PasswordEncripter _passwordEncripter;

        public RegisterUserUseCase(IUserReadOnlyRepository readOnlyRepository, IUserWriteOnlyRepository writeOnlyRepository, IMapper mapper, PasswordEncripter passwordEncripter, IUnitOfWork unitOfWork)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            // Validar as propriedades da request e lançar exceções.
            await Validate(request);

            // Mapear as propriedades da request com a entidade do domínio. Utilizando a biblioteca AutoMapper (injeçao de dependencia)
            var user = _mapper.Map<Domain.Entities.User>(request);

            // Encriptação da senha.
            user.Password = _passwordEncripter.Encrypt(request.Password);

            await _writeOnlyRepository.Add(user);

            await _unitOfWork.Commit();

            return new ResponseRegisteredUserJson
            {
                Name = request.Name
            };
        }

        public async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();
            var result = validator.Validate(request);

            var emailExist = await _readOnlyRepository.ExistAciveUserWithEmail(request.Email);
            if (emailExist)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            }

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
