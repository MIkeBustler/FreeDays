using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// Root structure
    /// </summary>
    public class Organization : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Equipment> Equipments { get; set; }
        public virtual ICollection<OrgStruct> OrgStructs { get; set; }
    }
}
