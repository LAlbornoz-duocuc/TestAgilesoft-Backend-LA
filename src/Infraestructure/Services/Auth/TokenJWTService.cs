using Infraestructure.Interfaces.Auth;
using Microsoft.IdentityModel.Tokens;
using Models.DTO.Api.Auth;
using Models.DTO.Infraestructure;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Services.Auth
{
    public class TokenJWTService : ITokenJWT
    {
        private static readonly string SecKey = "61M4dXz96vXuvqoBbwHKiRRXXghZ94pn";
        public TokenJWTService() { }

        public GenerateJWT GenerateToken(UsuarioForJWT Usuario)
        {
            DateTime fechaExpiracion = DateTime.Now.AddHours(12);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Usuario.Nombre),
                new Claim(ClaimTypes.NameIdentifier, Usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(fechaExpiracion).ToUnixTimeSeconds().ToString())
            };

            var token = new JwtSecurityToken(
                    new JwtHeader(
                            new SigningCredentials(
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecKey)),
                                SecurityAlgorithms.HmacSha256)),
                    new JwtPayload(claims));

            var output = new GenerateJWT
            {
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = Usuario.Id,
                FechaExp = fechaExpiracion
            };

            return output;
        }
    }
}
