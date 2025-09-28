using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext dbContext;

        public GenericRepository(StoreContext _dbContext)
            => dbContext = _dbContext;
        
        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await dbContext.Set<T>().ToListAsync();
        
        public async Task<T> GetByIdAsync(int id)
            =>await dbContext.Set<T>().FindAsync(id); 
      
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
            => await ApplySpecification(spec).ToListAsync();
        
        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
            =>await ApplySpecification(spec).SingleOrDefaultAsync();
        
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        
            =>SpecificationEvaluator<T>.GetQuery(dbContext.Set<T>(), spec);
        
        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
            => await ApplySpecification(spec).CountAsync();
        
        public async Task AddAsync(T entity)
            =>await dbContext.Set<T>().AddAsync(entity);
        
        public void Update(T entity)
            =>dbContext.Set<T>().Update(entity);
        
        public void Delete(T entity)
            =>dbContext.Set<T>().Remove(entity);
        
    }
}
