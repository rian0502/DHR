using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.Divison;

public class CreateDivisionViewModel
{
    [Required(ErrorMessage = "Division Code is required")]
    public required string DivisionCode { get; set; }
    [Required(ErrorMessage = "Division Name is required")]
    public required string DivisionName { get; set; }
    [Required(ErrorMessage = "Sub Department is required")]
    public required int SubDepartmentId { get; set; }
}