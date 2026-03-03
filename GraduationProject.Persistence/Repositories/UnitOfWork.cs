using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.persistence.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContexts _dbContexts;
        private readonly Dictionary<Type, Object> _repositories = [];
        public UnitOfWork(StoreDbContexts dbContexts)
        {
            _dbContexts = dbContexts;
        }
        //mafe4 ay block hy7sal 
        //3 steps 1)b3ml dictionary fe kol l repositories deh
        //2 lw tlab repo gowa l dic adeholo lw fe nafs l request
        //lw talab repository msh fe nafs l dictionarry habda2 a create leh repository dah w a7oto fl ditionary
        //34an lw talabo tani f nafs l request adeholo
        //hy3ml check howa l type bta3 l generic repo lli ana 3awzo mn type mo3aian  mawgod fl dictioanry elawel
        // y4ouf howa l type dah k key mawgod fl dictionar wla la
        //key => type //// value=> object mn l generic repo

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
