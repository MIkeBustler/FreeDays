using FreeDOW.API.Core.Abstract;
using FreeDOW.API.Core.Entities;
using FreeDOW.API.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FreeDOW.API.DataAccess.Repositries
{
    public  class EfRepository<T> : IRepository<T> where T: BaseEntity
    {
        private readonly AppDb _db;

        public EfRepository(AppDb db)
        {
            _db = db;
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            var data = await _db.Set<T>().Where(filter).ToListAsync();
            return data;
        }

        public async Task<T> GetByIDAsync(Guid Id)
        {
            var data = await _db.Set<T>().FirstOrDefaultAsync(rec=> rec.Id == Id);
            return data;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var data = await _db.Set<T>().FirstOrDefaultAsync(rec => rec.Id == entity.Id);
            if (data == null) return null;
            _db.Set<T>().Update(entity);
            _db.SaveChanges();
            return data;
        }

        public async Task DeleteAsync(T entity)
        {
            var data = await _db.Set<T>().FirstOrDefaultAsync(rec => rec.Id == entity.Id);
            if (data == null) return ;
            _db.Set<T>().Remove(entity);
            _db.SaveChanges();
        }
    }
}
