using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    /// base settings for ovarwork calculation
    /// </summary>
    public class Settings :BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }

    }
}
