using System.ComponentModel.DataAnnotations;

namespace DHR.Models;

public class EmployeeLeaveRequestModel
{
    [Key]
    public int EmployeeLeaveRequestId { get; set; }
    
    public DateOnly LeaveDate { get; set; }
    
    public double LeaveDays { get; set; }
    
    public string? LeaveReason { get; set; }

    /*
     * Leave Type:
     * Deducts Annual Leave: Regular Leave, Collective Leave
     * Does Not Deduct Annual Leave: Maternity Leave, Marriage Leave
     */
    public string? LeaveType { get; set; }

    
    public int EmployeeId { get; set; }
    
    
    //Relational Model
    public EmployeeModel? Employee { get; set; }
    
    
    //Log Attributes
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    
}