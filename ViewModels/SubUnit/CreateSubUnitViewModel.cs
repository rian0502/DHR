using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.SubUnit;

public class CreateSubUnitViewModel
{
    [Required(ErrorMessage = "SubUnit Code is required")]
    public required string SubUnitCode { get; set; }
    [Required(ErrorMessage = "SubUnit Name is required")]
    public required string SubUnitName { get; set; }
    [Required(ErrorMessage = "SubUnit Address is required")]
    public required string SubUnitAddress { get; set; }
    [Required(ErrorMessage = "Location is required")]
    public required int LocationId { get; set; }
    [Required(ErrorMessage = "Unit is required")]
    public required int UnitId { get; set; }
}