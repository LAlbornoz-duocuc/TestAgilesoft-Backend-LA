using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Confirmar transacción
        /// </summary>
        /// <returns>Numero de registros afectados</returns>
        Task<int> CommitAsync();
        IGenericRepository<T> GetRepository<T>()
            where T : BaseEntity;
    }
}
