using ECommerce.Domain.Contracts;

using GraduationProject.Domain.Entities;
using GraduationProject.Persistence.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContexts;
        private readonly Dictionary<Type, Object> _repositories = [];
        public UnitOfWork(StoreDbContext dbContexts)
        {
            _dbContexts = dbContexts;
        }
       

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var entityType = typeof(TEntity);//keda mskt esm l class
            //tryget value byrGa3 l kema mn l dic wl value deh no3ha aslun object
            if (_repositories.TryGetValue(entityType, out var repository))
            { 
                //5ali balak n l repository mn no3 object fana lezam a3mlo cast
                //object lli rage3 3awez a5od mno l goz2 lli by3ml implement ll igenericrepo
                return (IGenericRepository<TEntity, TKey>)repository;
            }
            var newRepo = new GenericRepository<TEntity, TKey>(_dbContexts);
            _repositories[entityType] = newRepo;
            return newRepo;
        }

        public async Task<int> SaveChangesAsync() 
     => await _dbContexts.SaveChangesAsync();
    }
}
