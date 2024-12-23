

using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.Department;

public class CreateDepartmentViewModel
{
    [Required(ErrorMessage = "Department Name is required")]
    public required string DepartmentName { get; set; }
    [Required(ErrorMessage = "Department Code is required")]
    public required string DepartmentCode { get; set; }
}