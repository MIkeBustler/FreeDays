using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// Free day claim
    /// </summary>
    public class FreeDay : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime DTStart { get; set; }
        public DateTime DTEnd { get; set; }
        public int Confirm { get; set; }

        public ICollection<OWFD> OWFDs { get; set; }
    }
}
