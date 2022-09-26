using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// Free day claim
    /// </summary>
    public class FreeDay : BaseEntity
    {
        public Guid EmployeeId { get; set; }

        public DateTime DTStart { get; set; }
        public DateTime DTEnd { get; set; }
        public int Confirmed { get; set; }
        public ICollection<OverWork> OverWorks { get; set; }
        public ICollection<OWFD> OWFDS { get;set; }

        public FreeDay()
        {
            OverWorks = new List<OverWork>();
            OWFDS = new List<OWFD>();
        }
    }
}
