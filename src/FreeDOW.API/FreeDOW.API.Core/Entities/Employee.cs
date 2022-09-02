using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// Employee entity
    /// </summary>
    public class Employee : BaseEntity
    {
        public Guid OrgStructId { get; set; }
        public OrgStruct OrgStruct { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [MaxLength(50), Required(ErrorMessage = "Отсутствует адрес электронной почты")]
        public string Email { get; set; }
        [MaxLength(20)]
        public string Login { get; set; }
        [MaxLength(20), MinLength(8), Required(ErrorMessage ="Отсутствует пароль")]
        public string Password { get; set; }
    }
}
