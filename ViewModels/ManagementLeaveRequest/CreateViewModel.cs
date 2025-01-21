using System.ComponentModel.DataAnnotations;

namespace DHR.ViewModels.ManagementLeaveRequest;

public class CreateViewModel
{
    [Required(ErrorMessage = "Please select an employee.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Please enter a leave code.")]
    public required string LeaveCode { get; set; }
    [Required(ErrorMessage = "Please select a leave date.")]
    public DateOnly LeaveDate { get; set; }
    [Required(ErrorMessage = "Please enter the number of leave days.")]
    public double LeaveDays { get; set; }
    [Required(ErrorMessage = "Please select a leave type.")]
    public required string LeaveType { get; set; }
    [Required(ErrorMessage = "Please enter the reason for leave.")]
    public required string LeaveReason { get; set; }
}