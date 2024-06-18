using Models.DTO.Api.Auth;
using Models.DTO.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Interfaces.Auth
{
    public interface ITokenJWT
    {
        public GenerateJWT GenerateToken(UsuarioForJWT Persona);
    }
}
