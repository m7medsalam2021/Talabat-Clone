using Talabat.Core;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private readonly Dictionary<Type, object> _repositories;


        public UnitOfWork(StoreContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>(); 
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            Type type = typeof(TEntity); 

            if (!_repositories.ContainsKey(type))
            {
                GenericRepository<TEntity>? repository = new GenericRepository<TEntity>(_context);
                _repositories.Add(type, repository);
            }
            return (IGenericRepository<TEntity>)_repositories[type]; 
        }

        public async Task<int> Complete()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DB: " + ex.InnerException?.Message ?? ex.Message);
                throw;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync(); 
        }

    }
}
