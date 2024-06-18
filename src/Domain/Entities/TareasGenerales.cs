using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TareasGenerales : BaseEntity
    {
        public string Nombre { get; set; }
        public string Descripción { get; set; }
        public int Estado { get; set; }//1: No Resuelto 2: Resuelto
        public DateTime FechaCreación { get; set; }
        public DateTime FechaUltimaActualizacion { get; set; }
    }
}
