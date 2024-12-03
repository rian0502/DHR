using System.ComponentModel.DataAnnotations;

namespace DAHAR.Models;

public class FormApplicationRequestModel
{
    [Key]
    public int FormId { get; set; }
    public string? FormCode { get; set; }
    public string? FormName { get; set; }
    public string? PathForm { get; set; }
    public string? Description { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}