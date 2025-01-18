using System.ComponentModel.DataAnnotations;

namespace DHR.Models;
public class EmployeePermissionRequest
{
    [Key]
    public int EmployeePermissionRequestId { get; set; }
    public string? EmployeePermissionRequestCode { get; set; }
    public DateOnly PermissionDate { get; set; }
    public double PermissionDays { get; set; }
    /*
     * Permission Type:
     * 1. Izin Terlambat
     * 2. Izin Pulang Cepat
     * 3. Izin Tidak Masuk Kerja
     * 4. Izin Keluar Kantor
     */
    public string? PermissionType { get; set; }
    public string? PermissionReason { get; set; }
    
    /*
     * Personnel Remarks:
     * Potong Cuti
     * Potong Gaji
     * Dispensasi
     */
    public string? PersonnelRemarks { get; set; }
    
    public int EmployeeId { get; set; }
    //Relational Model
    public EmployeeModel? Employee { get; set; }
    
    //Log Attributes
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string? DeleteReason { get; set; }

}

