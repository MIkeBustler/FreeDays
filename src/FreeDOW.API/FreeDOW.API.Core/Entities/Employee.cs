using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// Employee entity
    /// </summary>
    public class Employee : BaseEntity
    {
        public Guid OrgStructId { get; set; }
        public OrgStruct? OrgStruct { get; set; }
        
        [MaxLength(30)]
        public string? FirstName { get; set; }
        [MaxLength(30)]
        public string? LastName { get; set; }
        [MaxLength(50), Required(ErrorMessage = "Отсутствует адрес электронной почты")]
        public string? Email { get; set; }
        [MaxLength(20), Required(ErrorMessage = "Отсутствует логин")]
        public string? Login { get; set; }
        [MaxLength(20), MinLength(8), Required(ErrorMessage ="Отсутствует пароль")]
        public string? Password { get; set; }
        public ICollection<EmployeeRole> EmployeeRoles { get; set; }
        public string? InviteName { get; set; }

        public Employee()
        {
            EmployeeRoles = new List<EmployeeRole>();
        }
    }
}
