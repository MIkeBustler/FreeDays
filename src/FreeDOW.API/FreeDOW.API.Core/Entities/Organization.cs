using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// Root structure
    /// </summary>
    public class Organization : BaseEntity
    {
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public virtual ICollection<Equipment> Equipments { get; set; }
        public virtual ICollection<OrgStruct> OrgStructs { get; set; }

        public Organization()
        {
            Equipments = new List<Equipment>();
            OrgStructs = new List<OrgStruct>();
        }
    }
}
