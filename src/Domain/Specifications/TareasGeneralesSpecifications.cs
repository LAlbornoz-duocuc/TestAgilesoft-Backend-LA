using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class TareasGeneralesSpecifications : Specification<TareasGenerales>
    {
        public TareasGeneralesSpecifications(string UserName)
        {
            Query.Where(x => x.Username == UserName);
        }
    }
}
