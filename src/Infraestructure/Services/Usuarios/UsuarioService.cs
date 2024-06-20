using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services.Usuarios;
using Domain.Specifications;
using Infraestructure.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Services.Usuarios
{
    public class UsuarioService : IUsuarios
    {
        private readonly IUnitOfWork _Repository;
        private readonly IEncriptar _encriptar;

        public UsuarioService(IUnitOfWork repository, IEncriptar encriptar)
        {
            _Repository = repository;
            _encriptar = encriptar;
        }

        public async Task<int> AddUsuario(Usuario usuario)
        {
            try
            {
                var repo = _Repository.GetRepository<Usuario>();

                _encriptar.Encriptar(usuario.Password);
                usuario.Password = _encriptar.ObtenerHash();


                await repo.AddAsync(usuario).ConfigureAwait(false);
                int resultado = await _Repository.CommitAsync().ConfigureAwait(false);

                return resultado;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<Usuario> GetUsuarioById(int id)
        {
            try
            {
                var repo = _Repository.GetRepository<Usuario>();
                var resultado = await repo.GetByIdAsync(id).ConfigureAwait(false);

                return resultado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Usuario> GetUsuarioByUsername(string username)
        {
            try
            {
                var repo = _Repository.GetRepository<Usuario>();
                var spec = new UsuarioSpecifications(username);
                var Usuario = await repo.FirstOrDefaultAsync(spec).ConfigureAwait(false);

                return Usuario;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
