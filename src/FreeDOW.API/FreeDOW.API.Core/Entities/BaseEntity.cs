using System.ComponentModel.DataAnnotations;
namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// Base entity 
    /// </summary>
    public class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
