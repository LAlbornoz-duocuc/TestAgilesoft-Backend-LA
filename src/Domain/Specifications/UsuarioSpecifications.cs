using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class UsuarioSpecifications : Specification<Usuario>
    {
        public UsuarioSpecifications(string UserName)
        {
            Query.Where(x => x.Username == UserName);
        }
    }
}
