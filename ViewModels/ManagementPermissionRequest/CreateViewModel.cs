using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.ManagementPermissionRequest
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "Permission Code is required")]
        public required string EmployeePermissionRequestCode { get; set; }

        [Required(ErrorMessage = "Permission Date is required")]
        public required DateOnly PermissionDate { get; set; }

        [Required(ErrorMessage = "Permission Days is required")]
        public double PermissionDays { get; set; }

        [Required(ErrorMessage = "Permission Type is required")]
        public required string PermissionType { get; set; }
        
        [Required(ErrorMessage = "Permission Reason is required")]
        public required string PermissionReason { get; set; }
        
        [Required(ErrorMessage = "Personnel Remarks is required")]
        public required string PersonnelRemarks { get; set; }

        [Required(ErrorMessage = "Employee Id is required")]
        public int EmployeeId { get; set; }

    }
}
