namespace FreeDOW.API.WebHost.Models
{
    public class CreateOrEditOverWork
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid EquipmentId { get; set; }
        public DateTime DTStart { get; set; }
        public DateTime DTEnd { get; set; }
        public string? EquipmentDetail { get; set; }
        public string? Description { get; set; }
    }
}
