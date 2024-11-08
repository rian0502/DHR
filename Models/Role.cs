using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class Role
    {
        private readonly string _TABLE = "roles";
        [Key]
        public int RoleID { get; set; }
        public string? RoleName { get; set; }

        //Relational Model
        public ICollection<Users>? Users { get; set; } //Has many Users

        //Log Attributes
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
