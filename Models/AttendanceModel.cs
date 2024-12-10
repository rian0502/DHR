using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DAHAR.Models
{
    public class AttendanceModel
    {
        [Key]
        public int EmployeeId { get; set; }
        public byte Day { get; set; }
        public DateTime? Date { get; set; }
        public string? Code { get; set; }
        public decimal? CheckIn { get; set; }
        public decimal? CheckOut { get; set; }
        public decimal? Late { get; set; }
        public string? Note { get; set; }
        public short? MealAllowance { get; set; }
        public double? BenefitAmount { get; set; }
    }
}
