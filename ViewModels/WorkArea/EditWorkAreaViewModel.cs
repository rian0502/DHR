using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels.WorkArea
{
    public class EditWorkAreaViewModel
    {
        [Required(ErrorMessage = "Missing Required Edit Data")]
        public int LocationId { get; set; }
        [Required(ErrorMessage = "Code Location is required")]
        public string? LocationCode { get; set; }
        [Required(ErrorMessage = "Name Location is required")]
        public string? LocationName { get; set; }
    }
}
