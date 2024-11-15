using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class AppLogModel
    {
        [Key]
        public int AppLogId { get; set; }
        public string? Source { get; set; }
        public string? Params { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
