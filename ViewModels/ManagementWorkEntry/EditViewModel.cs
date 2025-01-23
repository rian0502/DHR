using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.ManagementWorkEntry
{
    public class EditViewModel
    {
        [Required(ErrorMessage = "Missing Field required")]
        public int EmployeeWorkEntryId { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public required string EmployeeWorkEntryCode { get; set; }

        [Required(ErrorMessage = "Work Date is required")]
        public DateOnly WorkDate { get; set; }

        [Required(ErrorMessage = "Start Time is required")]
        public TimeOnly WorkStartTime { get; set; }
        [Required(ErrorMessage = "End Time is required")]
        public TimeOnly WorkEndTime { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        public required string WorkReason { get; set; }

        [Required(ErrorMessage = "Remark is required")]
        public string PersonnelRemark { get; set; }

        [Required(ErrorMessage = "Employee is required")]
        public int EmployeeId { get; set; }
    }
}
