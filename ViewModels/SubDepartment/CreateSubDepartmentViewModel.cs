
using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.SubDepartment;

public class CreateSubDepartmentViewModel
{
    [Required(ErrorMessage = "Sub Department Name is required")]
    public required string SubDepartmentName { get; set; }
    [Required(ErrorMessage = "Sub Department Code is required")]
    public required string SubDepartmentCode { get; set; }
    [Required(ErrorMessage = "Department is required")]
    public required int DepartmentId { get; set; }
}