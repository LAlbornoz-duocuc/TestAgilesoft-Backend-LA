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
        public TareasGeneralesSpecifications(List<int> TareasGeneralesIds)
        {
            Query.Where(x => TareasGeneralesIds.Contains(x.Id));
        }
    }
}
