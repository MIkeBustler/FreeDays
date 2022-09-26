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
    /// <summary>
    /// Employee repository. Since we don't cnahge collection for db store
    /// use AsNoTracking for lower resources using
    /// </summary>
    public class EmployeeRepository : OrgRepository<Employee>
    {
        private readonly DbSet<Employee> _db;

        public EmployeeRepository(OrgDbContext db) : base(db)
        {
            _db = db.Set<Employee>();
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var res = await _db.AsNoTracking()
                .Include(rec => rec.EmployeeRoles)
                .ThenInclude(rec => rec.Role)
                .Include(rec=>rec.OrgStruct)
                .ToListAsync();
            return res;
        }


        public override async Task<IEnumerable<Employee>> GetByConditionAsync(Expression<Func<Employee, bool>> filter)
        {
            var res = await _db.AsNoTracking()
                .Where(filter)
                .Where(rec=>!rec.IsDeleted)
                .Include(rec => rec.EmployeeRoles)
                .ThenInclude(rec => rec.Role)
                .Include(rec => rec.OrgStruct)
                .ToListAsync();
            return res;

        }

        public override async Task<Employee> GetByIdAsync(Guid id)
        {
            var res = await _db.AsNoTracking()
                .Include(rec => rec.EmployeeRoles)
                .ThenInclude(rec => rec.Role)
                .Include(rec => rec.OrgStruct)
                .FirstOrDefaultAsync(rec => rec.Id == id);
            return res;
        }
    }
}
