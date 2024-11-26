using DAHAR.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class EmployeeController(EmployeeService employeeService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var employees = await employeeService.FindAll();
        return View(employees);
    }
}