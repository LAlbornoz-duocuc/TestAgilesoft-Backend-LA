using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        /*atributos genericos de la base de datos*/
        public virtual int Id { get; set; }
    }
}
