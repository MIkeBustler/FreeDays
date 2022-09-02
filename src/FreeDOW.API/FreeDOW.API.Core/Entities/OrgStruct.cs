using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// organization structure for the given organization
    /// </summary>
    public class OrgStruct : BaseEntity
    {
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public ICollection<Employee> Employees { get; set; }

    }
}
