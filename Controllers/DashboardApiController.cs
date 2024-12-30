using DHR.Helper;
using DHR.Models;
using DHR.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DHR.Controllers;

public class DashboardApiController(
    UserManager<Users> userManager,
    YearPeriodService yearPeriodService,
    AppDbContext context,
    DashboardService dashboardService) : Controller
{
    // GET
    public async Task<IActionResult> YearPeriods()
    {
        var yearPeriods = await yearPeriodService.GetYearPeriod();
        return Ok(new
        {
            periods = yearPeriods
        });
    }

    public async Task<IActionResult> AllowanceData(string yearPeriod)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest(new { Status = false, Message = "User not found" });
        }

        var employee = await context.Employee
            .FirstOrDefaultAsync(e => e.UserId == user.Id);
        if (employee == null)
        {
            return Unauthorized(new { Status = false, Message = "Unauthorized, Please Logout..." });
        }

        var allowance = await dashboardService.GetAllowance(yearPeriod, employee.Nip);
        return Ok(allowance);
    }
    
    public async Task<IActionResult> AttendanceData(string yearPeriod)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest(new { Status = false, Message = "User not found" });
        }

        var employee = await context.Employee
            .FirstOrDefaultAsync(e => e.UserId == user.Id);
        if (employee == null)
        {
            return Unauthorized(new { Status = false, Message = "Unauthorized, Please Logout..." });
        }
        var attendance = await dashboardService.GetAttendance(yearPeriod, employee.Nip);
        return Ok(attendance);
    }
    
    public async Task<IActionResult> LeaveRequestData(string yearPeriod)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest(new { Status = false, Message = "User not found" });
        }

        var employee = await context.Employee
            .FirstOrDefaultAsync(e => e.UserId == user.Id);
        if (employee == null)
        {
            return Unauthorized(new { Status = false, Message = "Unauthorized, Please Logout..." });
        }
        var attendance = await dashboardService.GetLeaveRequest(yearPeriod, employee.Nip);
        return Ok(attendance);
    }
    
    public async Task<IActionResult> MedicalClaimData(string yearPeriod)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest(new { Status = false, Message = "User not found" });
        }

        var employee = await context.Employee
            .FirstOrDefaultAsync(e => e.UserId == user.Id);
        if (employee == null)
        {
            return Unauthorized(new { Status = false, Message = "Unauthorized, Please Logout..." });
        }
        var attendance = await dashboardService.GetMedicalClaim(yearPeriod, employee.Nip);
        return Ok(attendance);
    }
}