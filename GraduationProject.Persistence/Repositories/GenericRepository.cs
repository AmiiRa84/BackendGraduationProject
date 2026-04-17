using ECommerce.Domain.Contracts;

using GraduationProject.Domain.Entities;
using GraduationProject.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _dbContexts;
        //clr fl mvc mkn4 howa lli byedene l generic repo e7na kona
        //3amlen method gowa l iunit of work btedene l repository 3la 7db mana 3awez
        public GenericRepository(StoreDbContext dbContexts)
        {
            _dbContexts = dbContexts;
        }
      
        public async Task AddAsync(TEntity entity)
        {
            await _dbContexts.Set<TEntity>().AddAsync(entity);


        }

        public void Delete(TEntity entity)
        {
            _dbContexts.Set<TEntity>().Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() 
            => await _dbContexts.Set<TEntity>().ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null
    )


        {
            IQueryable<TEntity> query = _dbContexts.Set<TEntity>();
            if (include != null)
                query = include(query);
            return await query.ToListAsync();
        }

        //find btdawar lawel fl local lw mal2t4 btro7 3la l db fkeda db hits
        public async Task<TEntity?> GetByIdAsync(TKey id)
            => await _dbContexts.Set<TEntity>().FindAsync(id);

        public void Update(TEntity entity)
        {
            //from unmodified to modified
            _dbContexts.Set<TEntity>().Update(entity);
        }
    }
}
