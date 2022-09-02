using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    ///  auxilary records for user register and email confirmation
    /// </summary>
    public class RegUserEntry :BaseEntity
    {
        public Guid OrgStructId { get; set; }
        public Organization Organization { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Guid RegCode { get; set; }
        public DateTime RegDate { get; set; }
        public string Email { get; set; }
    }
}
