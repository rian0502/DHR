using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class PeriodModel
    {
        [Key]
        public int PeriodId { get; set; }
        public DateTime? StartPeriodDate { get; set; }
        public DateTime? EndPeriodDate { get; set; }
        public bool IsActive { get; set; }

        //Log Attr
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
