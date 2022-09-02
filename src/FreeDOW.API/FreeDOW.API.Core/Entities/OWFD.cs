using System.ComponentModel.DataAnnotations;

namespace FreeDOW.API.Core.Entities
{
    /// <summary>
    ///  how many time taken for freeday from overwork claim
    /// </summary>
    public class OWFD
    {
        public Guid OverWorkId { get; set; }
        public OverWork OverWork { get; set; }
        public Guid FreeDayId { get; set; }
        public FreeDay FreeDay { get; set; }
        public int TimeTaken { get; set; }
    }
}
