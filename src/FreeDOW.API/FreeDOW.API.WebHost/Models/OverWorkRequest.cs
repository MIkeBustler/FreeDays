namespace FreeDOW.API.WebHost.Models
{
    public class OverWorkRequest
    {
        public Guid EmployeeId { get; set; }
        public DateTime DTFrom { get; set; }
        public DateTime DTTo { get; set; }
    }
}
