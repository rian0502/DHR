using System.ComponentModel.DataAnnotations;

namespace Presensi360.ViewModels
{
    public class EditWorkAreaViewModel
    {
        [Required(ErrorMessage = "Missing Required Edit Data")]
        public int LocationID { get; set; }
        [Required(ErrorMessage = "Code Location is required")]
        public string? LocationCode { get; set; }
        [Required(ErrorMessage = "Name Location is required")]
        public string? LocationName { get; set; }
    }
}
