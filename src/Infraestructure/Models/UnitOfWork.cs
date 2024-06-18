using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _dbContext;
        private readonly Dictionary<Type, object> repos = new Dictionary<Type, object>();

        public UnitOfWork(Context dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<int> CommitAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity
        {
            var type = typeof(T);
            if (!repos.ContainsKey(type))
            {
                repos[type] = new RepositoryImplements<T>(_dbContext);
            }
            return (IGenericRepository<T>)repos[type];
        }
    }
}
