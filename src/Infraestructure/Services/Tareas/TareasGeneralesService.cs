using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services.TareasUsuarios;
using Domain.Interfaces.Services.Usuarios;
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

        public Task<int> CambiarEstadoTarea(int Estado, int TareasGeneralesId)
        {
            throw new NotImplementedException();
        }

        public Task<List<TareasGenerales>> GetTareasGenerales(List<int> ListaTareasGeneralesIds)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateTarea(TareasGenerales tareasGenerales)
        {
            throw new NotImplementedException();
        }
    }
}
