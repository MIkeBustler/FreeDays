using FreeDOW.API.Core.Abstract;
using FreeDOW.API.Core.Entities;
using FreeDOW.API.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FreeDOW.API.DataAccess.Repositries
{
    public class WorkTimeManageRepository<T> : IWorkTimeManageRepository<T> where T : BaseEntity
    {
        private readonly WorkTimeManageDbContext _db;

        public WorkTimeManageRepository(WorkTimeManageDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var data = await _db.Set<T>()
                .AsNoTracking()
                .Where(rec=>!rec.IsDeleted)
                .ToListAsync();
            return data;
        }

        public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> filter)
        {
            var data = await _db.Set<T>()
                .AsNoTracking()
                .Where(rec=>!rec.IsDeleted)
                .Where(filter)
                .ToListAsync();
            return data;
        }

        public async Task<T> GetByIdAsync(Guid Id)
        {
            var data = await _db.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(rec => rec.Id == Id);
            return data;
        }

        public async Task<T> CreateAsync(T entity)
        {
            entity.IsDeleted = false;
            await Task.Run(() => _db.Set<T>().Add(entity));
            _db.SaveChanges();
            return entity;
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
            if (data == null) return;
            _db.Set<T>().Remove(data);
            _db.SaveChanges();
        }
    }
}
