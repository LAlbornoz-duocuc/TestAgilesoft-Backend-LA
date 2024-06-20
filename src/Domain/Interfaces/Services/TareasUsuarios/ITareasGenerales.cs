using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.TareasUsuarios
{
    public interface ITareasGenerales
    {
        public Task<int> AddTarea(TareasGenerales tareasGenerales);
        public Task<int> UpdateTarea(TareasGenerales tareasGenerales);
        public Task<List<TareasGenerales>> GetTareasGeneralesByIds(List<int> ListaTareasGeneralesIds);
        public Task<List<TareasGenerales>> GetTareasGenerales();
        public Task<TareasGenerales> GetByTareasGeneralesId(int TareasGeneralesId);
    }
}
