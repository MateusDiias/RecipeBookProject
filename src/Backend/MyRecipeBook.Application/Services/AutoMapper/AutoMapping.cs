using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.Services.AutoMapper
{
    // Biblioteca AutoMapper, mapeando a entidade no dominio.
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
            DomainToResponse();
        }

        // mapeia automaticamente a requisicao para o dominio, nao mapeia a senha.
        private void RequestToDomain()
        {
            CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
                .ForMember(destine => destine.Password, option => option.Ignore());
        }

        private void DomainToResponse()
        {
            CreateMap<Domain.Entities.User, ResponseUserProfileJson>();
        }
    }
}
