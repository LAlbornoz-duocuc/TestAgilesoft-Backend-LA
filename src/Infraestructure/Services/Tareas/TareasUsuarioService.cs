using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services.TareasUsuarios;
using Domain.Interfaces.Services.Usuarios;
using Domain.Specifications;
using Infraestructure.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Services.Tareas
{
    public class TareasUsuarioService : ITareasUsuario
    {
        private readonly IUnitOfWork _Repository;
        public TareasUsuarioService(IUnitOfWork repository)
        {
            _Repository = repository;
        }
        public async Task<int> AddTareaUsuario(TareasUsuario tareasUsuario)
        {
            try
            {
                var repo = _Repository.GetRepository<TareasUsuario>();


                await repo.AddAsync(tareasUsuario).ConfigureAwait(false);
                int resultado = await _Repository.CommitAsync().ConfigureAwait(false);

                return resultado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> DeleteTareaUsuario(TareasUsuario tareasUsuario)
        {
            try
            {
                var repo = _Repository.GetRepository<TareasUsuario>();
                await repo.DeleteAsync(tareasUsuario).ConfigureAwait(false);
                var resultado = await _Repository.CommitAsync().ConfigureAwait(false);

                return resultado;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<List<TareasUsuario>> GetTareasByUserId(TareasUsuarioSpecifications tareasUsuarioSpecifications)
        {
            try
            {
                var repo = _Repository.GetRepository<TareasUsuario>();
                var list = await repo.ListAsync(tareasUsuarioSpecifications).ConfigureAwait(false);

                return list.ToList();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<TareasUsuario> GetTareasByUserIdAndTaskId(TareasUsuarioSpecifications tareasUsuarioSpecifications)
        {
            try
            {
                var repo = _Repository.GetRepository<TareasUsuario>();
                var result = await repo.FirstOrDefaultAsync(tareasUsuarioSpecifications).ConfigureAwait(false);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
