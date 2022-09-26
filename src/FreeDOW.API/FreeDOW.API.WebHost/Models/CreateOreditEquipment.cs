namespace FreeDOW.API.WebHost.Models
{
    public class CreateOreditEquipment
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        
        public string Name { get; set; }
    }
}
