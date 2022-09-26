using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// this setting oveerride organization settings and default if any
    /// </summary>
    public class EmpSettingValue: BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public Guid SettingId { get; set; }
        public Settings? Settings { get; set; }
        public string? Value { get; set; }
        
    }
}
