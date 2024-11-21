using System.ComponentModel.DataAnnotations;
namespace DAHAR.ViewModels.Education;

public class CreateEducationViewModel
{
    [Required(ErrorMessage = "Education Name is required")]
    public required string EducationName { get; set; }
}