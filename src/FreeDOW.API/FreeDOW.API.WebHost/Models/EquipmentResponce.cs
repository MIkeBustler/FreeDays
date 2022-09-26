namespace FreeDOW.API.WebHost.Models
{
    public class EquipmentResponce
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string Name { get;set;}
        public bool IsDeleted { get; set; } = false;
    }
}
