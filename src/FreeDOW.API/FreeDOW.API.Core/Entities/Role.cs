using System.ComponentModel.DataAnnotations;
namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// roles for users
    /// </summary>
    public class Role :BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<EmployeeRole> EmployeeRoles { get; set; }

        public Role()
        {
            EmployeeRoles = new List<EmployeeRole>();
        }
    }

    
}
