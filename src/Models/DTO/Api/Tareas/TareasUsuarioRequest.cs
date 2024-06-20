using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Api.Tareas
{
    public class TareasUsuarioRequest
    {
        public int TareasId { get; set; }
        public int UsuarioId { get; set; }
    }
}
