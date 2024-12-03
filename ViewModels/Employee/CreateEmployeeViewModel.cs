using System.ComponentModel.DataAnnotations;

namespace DAHAR.ViewModels.Employee;

public class CreateEmployeeViewModel
{

    [Required(ErrorMessage = "NIP is required")]
    [MaxLength(4, ErrorMessage = "NIP must be 4 characters")]
    public required string Nip { get; set; }
    
    [Required(ErrorMessage = "NIK is required")]
    [MaxLength(16, ErrorMessage = "NIK must be 16 characters")]
    public required string Nik { get; set; }
    
    [Required(ErrorMessage = "NPWP is required")]
    public required string Npwp { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    public required string FullName { get; set; }
    
    [Required(ErrorMessage="Gender is required")]
    public required string Gender { get; set; }
    
    [Required(ErrorMessage="Address is required")]
    public required string Address { get; set; }
    
    [Required(ErrorMessage="Join Date is required")]
    public required DateTime JoinDate { get; set; }
    
    [Required(ErrorMessage = "Phone number is required")]
    public required string PhoneNumber { get; set; }
    
    [Required(ErrorMessage="Division is required")]
    public required int DivisionId { get; set; }    
    [Required(ErrorMessage="Job Title is required")]
    public required int JobTitleId { get; set; }
    
    [Required(ErrorMessage="Tax is required")]
    public required int TaxExemptIncomeId { get; set; }
    
    [Required(ErrorMessage="Sub-Unit is required")]
    public required int SubUnitId { get; set; }
    
    [Required(ErrorMessage="Religion is required")]
    public required int ReligionId { get; set; }
    
    [Required(ErrorMessage="Education is required")]
    public required int EducationId { get; set; }
    
}