using System.ComponentModel.DataAnnotations;

namespace DHR.Models;

public class EmployeeWorkEntryRequest
{
    [Key]
    public int EmployeeWorkEntryId { get; set; }
    public string? EmployeeWorkEntryCode { get; set; }
    public DateOnly? WorkDate { get; set; }
    public TimeOnly? WorkStartTime { get; set; }
    public TimeOnly? WorkEndTime { get; set; }
    public string? WorkReason { get; set; }
    public string? PersonnelRemark { get; set; }
    public int EmployeeId { get; set; }
    
    //Relationships
    public EmployeeModel? Employee { get; set; }
    
    //Log Attributes
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string? DeleteReason { get; set; }
    
}