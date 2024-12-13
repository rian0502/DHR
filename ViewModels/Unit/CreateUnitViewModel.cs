using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.Unit
{
    public class CreateUnitViewModel
    {
        [Required(ErrorMessage = "Unit Code is Required")]
        public required string UnitCode { get; set; }
        [Required(ErrorMessage = "Unit Name is Required")]
        public required string UnitName { get; set; }

    }
}
