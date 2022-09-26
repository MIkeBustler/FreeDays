namespace FreeDOW.API.WebHost.Models
{
    public class EmployeeResponce
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string InviteName { get; set; } = "";
        public Guid OrgStrId { get; set; }
        public string OrgStrName { get; set; } = "";
    }
}
