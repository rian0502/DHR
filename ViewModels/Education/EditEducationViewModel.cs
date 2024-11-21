
using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels.Education;

public class EditEducationViewModel
{
    [Required(ErrorMessage = "Missing Required Field")]
    public required int EducationId { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public required string EducationName { get; set; }
}