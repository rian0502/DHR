using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels.Period;

public class EditPeriodViewModel
{
    [Required(ErrorMessage = "Required")]
    public int PeriodId { get; set; }
    [Required(ErrorMessage = "Start Period is required")]
    public DateTime StartPeriodDate { get; set; }
    [Required(ErrorMessage = "End Period is required")]
    public DateTime EndPeriodDate { get; set; }
    
    public bool IsActive { get; set; }
}