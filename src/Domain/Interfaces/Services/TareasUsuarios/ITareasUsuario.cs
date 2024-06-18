using Domain.Entities;
using Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.TareasUsuarios
{
    public interface ITareasUsuario
    {
        public Task<int> AddTareaUsuario(TareasUsuario tareasUsuario);
        public Task<int> DeleteTareaUsuario(TareasUsuario tareasUsuario);
        public Task<List<TareasUsuario>> GetTareasByUserId(TareasUsuarioSpecifications tareasUsuarioSpecifications);
        
    }
}
