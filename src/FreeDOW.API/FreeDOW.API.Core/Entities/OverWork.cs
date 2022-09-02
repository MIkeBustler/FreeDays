using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    ///  Overwork claim
    /// </summary>
    public class OverWork :BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public Guid EquipmentId { get; set; }
        public Equipment Equipment { get; set; }
        public DateTime DTStart { get; set; }
        public DateTime DTEnd { get; set; }
        public string EquipmentDetail { get; set; }
        public string Desription { get; set; }
        public int Confirm { get; set; }
        public int reminder { get; set; }

        public ICollection<OWFD> OWFDs { get; set; }
        
    }
}
