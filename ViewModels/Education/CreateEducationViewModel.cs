using System.ComponentModel.DataAnnotations;
namespace DHR.ViewModels.Education;

public class CreateEducationViewModel
{
    [Required(ErrorMessage = "Education Name is required")]
    public required string EducationName { get; set; }
}