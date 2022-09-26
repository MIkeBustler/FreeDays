using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// Vacation Claim
    /// Confirmed values first bit set -confirmed by dept head, 2 bti set - confirmed by HR (locked)
    /// VacationType - link to the setting table record
    /// </summary>
    public class Vacation :BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public DateTime DTStart { get; set; }
        public DateTime DTEnd { get; set; }
        public int Confirmed { get; set; }
        public Guid VacationType { get; set; }
        public DateTime FactDTStart { get; set; }
        public DateTime FactDTEnd { get; set; }
    }
}
