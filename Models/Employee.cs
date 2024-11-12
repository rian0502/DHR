using System.ComponentModel.DataAnnotations;

namespace Presensi360.Models
{
    public class Employee
    {
        private readonly string _TABLE = "employee";
        [Key]
        public int? EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public string? UserId { get; set; }
        public int? JobTitleID { get; set; }
        public int? CompanyID { get; set; }
        public int? SubDepartmentID { get; set; }

        //Relational Model
        public Users? User { get; set; } //Belongs to Users
        public JobTitle? JobTitle { get; set; } //Belongs to JobTitle
        public CompanyModel? Company { get; set; } //Belongs to CompanyModel
        public SubDepartment? SubDepartment { get; set; } //Belongs to SubDepartment

    }
}
