using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels.Divison;

public class EditDivisionViewModel
{
    [Required(ErrorMessage = "Missing Required Attribute")]
    public int DivisionId { get; set; }
    [Required(ErrorMessage = "Division Code is required")]
    public required string DivisionCode { get; set; }
    [Required(ErrorMessage = "Division Name is required")]
    public required string DivisionName { get; set; }
    [Required(ErrorMessage = "Sub Department is required")]
    public required int SubDepartmentId { get; set; }
}