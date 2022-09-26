using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// settings than apply for organiazation position oveeride default settings
    /// </summary>
    public class OrgSettingValue :BaseEntity
    {
        public Guid SettingId { get; set; }
        public Settings? Settings { get; set; }
        public Guid OrgStructId { get; set; }
        public OrgStruct? OrgStruct { get; set; }
        public string? Value { get; set; }
    }
}
