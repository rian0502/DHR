using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels.Unit;

public class EditUnitViewModel
{
    [Required(ErrorMessage = "Missing Attribute")]
    public required int UnitId { get; set; }
    [Required(ErrorMessage = "Unit Code is Required")]
    public required string UnitCode { get; set; }
    [Required(ErrorMessage = "Unit Name is Required")]
    public required string UnitName { get; set; }
}