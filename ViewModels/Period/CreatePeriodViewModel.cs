using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels.Period;

public class CreatePeriodViewModel
{
    [Required(ErrorMessage = "Start Period is required")]
    public DateTime StartPeriodDate { get; set; }
    [Required(ErrorMessage = "End Period is required")]
    public DateTime EndPeriodDate { get; set; }
    
    public bool IsActive { get; set; }
}