using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// Equipment tree for the given organization
    /// </summary>
    public class Equipment :BaseEntity
    {
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization? Organization { get; set; }
    }
}
