using System.ComponentModel.DataAnnotations;

namespace DHR.Models
{
    public class AttendanceStatusModel
    {
        [Key]
        public int AttendanceStatusId { get; set; }
        public string? AttendanceStatusName { get; set; }
        public string? AttendanceStatusCode { get; set; }

        //Log Attributes
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeleteReason { get; set; }
    }
}
