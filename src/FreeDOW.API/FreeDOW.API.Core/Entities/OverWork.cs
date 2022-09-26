using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    ///  Overwork claim, register time for real work. 
    ///  Overhead time (in minutes) is taken accordoing to settings
    /// </summary>
    public class OverWork :BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public Guid EquipmentId { get; set; }
        public DateTime DTStart { get; set; }
        public DateTime DTEnd { get; set; }
        public int OverheadAddTime { get; set; } 
        public string? EquipmentDetail { get; set; }
        public string? Desription { get; set; }
        public int Confirmed { get; set; }
        public int Reminder { get; set; }
        public ICollection<FreeDay> FreeDays { get; set; }
        public ICollection<OWFD> OWFDS { get; set; }
        
        public OverWork()
        {
            FreeDays = new List<FreeDay>();
            OWFDS = new List<OWFD>();
        }
    }
}
