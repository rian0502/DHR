using System.ComponentModel.DataAnnotations;

namespace DAHAR.Models;

public class BenefitModel
{
    [Key]
    public int BenefitId { get; set; }
    public string? BenefitName { get; set; }
    public string? BenefitDescription { get; set; }
    public bool IsActive { get; set; }
    public bool IsMonetary { get; set; }
    // Examples: hourly, daily, weekly, monthly, yearly
    public string? Category { get; set; }
    
    
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    
}