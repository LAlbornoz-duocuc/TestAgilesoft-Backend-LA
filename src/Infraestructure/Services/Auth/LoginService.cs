using Domain.Interfaces;
using Domain.Interfaces.Services.Auth;
using Domain.Interfaces.Services.Usuarios;
using Infraestructure.Interfaces.Auth;
using Models.DTO.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Services.Auth
{
    public class LoginService : ILogin
    {
        private readonly IEncriptar _encriptar;
        private readonly IUnitOfWork _repository;
        private readonly IUsuarios _usuario;

        public LoginService(IEncriptar encriptar, IUnitOfWork repository, IUsuarios usuario)
        {
            _encriptar = encriptar;
            _repository = repository;
            _usuario = usuario;

        }
        /// <summary>
        /// Metodo que verifica si existe el usuario y coinciden las contraseñas
        /// </summary>
        /// <param name="usuarioLogin"></param>
        /// <returns>retorna un true si el usuario y contraseña existen o false si no coinciden</returns>
        public async Task<bool> ValidarCredenciales(UsuarioLogin usuarioLogin)
        {

            var usuario = await _usuario.GetUsuarioByUsername(usuarioLogin.Username).ConfigureAwait(false);

            if (usuario == null)
            {
                return false;
            }

            bool VerificarHash = _encriptar.VerificarHash(usuarioLogin.Password, usuario.Password);

            if (!VerificarHash)
            {
                return false;
            }



            return true;

        }
    }
}
