namespace FreeDOW.API.WebHost.Models
{
    public class OverWorkResponce
    {

        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid EquipmentId { get; set; }
        public DateTime DTStart { get; set; }
        public DateTime DTEnd { get; set; }
        public string? EquipmentPath { get; set; }
        public string? EquipmentDetail { get; set; }
        public string? Description { get; set; }
        public int Confirmed { get; set; }
        public int Reminder { get; set; }
    }
}
