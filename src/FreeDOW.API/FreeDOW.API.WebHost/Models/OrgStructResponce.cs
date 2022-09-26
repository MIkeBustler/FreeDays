namespace FreeDOW.API.WebHost.Models
{
    public class OrgStructResponce
    {
        public Guid Id { get; set; }
        public Guid ParentId {get; set;}
        public Guid OrgId { get; set;}
        public string Name { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}
