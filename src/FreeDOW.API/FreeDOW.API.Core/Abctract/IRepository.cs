using FreeDOW.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FreeDOW.API.Core.Abstract
{
    public interface IRepository<T> where T :BaseEntity

    {
        public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter);

        public Task<T> GetByIDAsync(Guid Id);

        public Task<T> UpdateAsync(T entity);

        public Task DeleteAsync(T entity);

    }
}
