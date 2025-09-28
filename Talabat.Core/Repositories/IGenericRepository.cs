using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity // BaseEntity : Parent for All Models
    {
        #region Static Way
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);
        #endregion


        #region Dynamic Way
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);
        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);
        #endregion



        #region Crud Operation
        Task AddAsync(T entity); 
        void Update(T entity);
        void Delete(T entity); 
        #endregion
    }
}
