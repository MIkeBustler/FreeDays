using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeDOW.API.Core.Entities
{
    public class EmployeeRole
    {
        public Guid EmployeeID { get; set; }
        public Employee? Employee { get; set; }
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
