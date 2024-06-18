using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TareasUsuario : BaseEntity
    {
        public int TareasId { get; set; }
        public int UsuarioId { get; set; }

    }
}
