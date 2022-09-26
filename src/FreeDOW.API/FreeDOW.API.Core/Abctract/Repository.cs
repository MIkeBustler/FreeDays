using FreeDOW.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FreeDOW.API.Core.Abstract
{
   

    public interface IOrgRepository<T>  where T : BaseEntity
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> filter);
        public Task<T> GetByIdAsync(Guid Id);
        public Task<T> CreateAsync(T entity);
        public Task<T> UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
    };

    public interface IWorkTimeManageRepository<T> where T : BaseEntity
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> filter);
        public Task<T> GetByIdAsync(Guid Id);
        public Task<T> CreateAsync(T entity);
        public Task<T> UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
    };
}
