
using Models.DTO.Domain;

namespace Domain.Interfaces.Services.Auth
{
    public interface ILogin
    {
        public Task<bool> ValidarCredenciales(UsuarioLogin usuarioLogin);
    }
}
