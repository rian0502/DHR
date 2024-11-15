using System.ComponentModel.DataAnnotations;

namespace Presensi360.ViewModels;

public class CreateWorkAreaViewModel
{
    [Required(ErrorMessage = "Code Location is required")]
    public string LocationCode { get; set; }
    [Required(ErrorMessage = "Name Location is required")]
    public string? LocationName { get; set; }
}