using AutoMapper;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.Services.AutoMapper
{
    // Biblioteca AutoMapper, mapeando a entidade no dominio.
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
        }

        // mapeia automaticamente a requisicao para o dominio, nao mapeia a senha.
        private void RequestToDomain()
        {
            CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
                .ForMember(destine => destine.Password, option => option.Ignore());
        }
    }
}
