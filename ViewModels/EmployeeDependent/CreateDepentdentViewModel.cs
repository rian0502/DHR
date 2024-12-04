using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels.EmployeeDependent;

public class CreateDepentdentViewModel
{
    [Required(ErrorMessage = "Name is required")]
    public required string DependentName { get; set; }
    [Required(ErrorMessage= "Gender is required")]
    public required string DependentGender { get; set; }
    [Required(ErrorMessage = "Status is required")]
    public required string DependentStatus { get; set; }
}