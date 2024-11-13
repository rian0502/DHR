using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }

    }
}
