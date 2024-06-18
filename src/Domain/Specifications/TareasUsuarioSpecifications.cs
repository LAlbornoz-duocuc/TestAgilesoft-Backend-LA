using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class TareasUsuarioSpecifications : Specification<TareasUsuario>
    {
        public TareasUsuarioSpecifications(int UsuarioId)
        {
            Query.Where(x => x.UsuarioId == UsuarioId);
        }
    }
}
