using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class Users
    {
        private readonly string _TABLE = "users";
        [Key]
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Status { get; set; }

        //Relational Model
        public Employee? Employee { get; set; } //Has one Employee
        public ICollection<Role>? Roles { get; set; } //Has many Roles


        //Attributes Log
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
