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
    public class TareasGeneralesService : ITareasGenerales
    {

        private readonly IUnitOfWork _Repository;

        public TareasGeneralesService(IUnitOfWork repository)
        {
            _Repository = repository;
        }
        public async Task<int> AddTarea(TareasGenerales tareasGenerales)
        {
            try
            {
                var repo = _Repository.GetRepository<TareasGenerales>();


                await repo.AddAsync(tareasGenerales).ConfigureAwait(false);
                int resultado = await _Repository.CommitAsync().ConfigureAwait(false);

                return resultado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TareasGenerales> GetByTareasGeneralesId(int TareasGeneralesId)
        {
            try
            {
                var repo = _Repository.GetRepository<TareasGenerales>();


                var resultado = await repo.GetByIdAsync(TareasGeneralesId).ConfigureAwait(false);


                return resultado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TareasGenerales>> GetTareasGenerales()
        {
            try
            {
                var repo = _Repository.GetRepository<TareasGenerales>();
                var list = await repo.ListAllAsync().ConfigureAwait(false);

                return list?.ToList() ?? new List<TareasGenerales>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TareasGenerales>> GetTareasGeneralesByIds(List<int> ListaTareasGeneralesIds)
        {
            try
            {
                var repo = _Repository.GetRepository<TareasGenerales>();
                var spec = new TareasGeneralesSpecifications(ListaTareasGeneralesIds);
                var list = await repo.ListAsync(spec).ConfigureAwait(false);

                return list?.ToList() ?? new List<TareasGenerales>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> UpdateTarea(TareasGenerales tareasGenerales)
        {
            try
            {
                var repo = _Repository.GetRepository<TareasGenerales>();


                await repo.UpdateAsync(tareasGenerales).ConfigureAwait(false);
                int resultado = await _Repository.CommitAsync().ConfigureAwait(false);

                return resultado;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
