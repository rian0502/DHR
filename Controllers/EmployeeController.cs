using DAHAR.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class EmployeeController(EmployeeService employeeService) : Controller
{
    // GET: EmployeeController/Index
    public async Task<IActionResult> Index()
    {
        var employees = await employeeService.FindAll();
        return View(employees);
    }
    
    // GET: EmployeeController/Details/id
    public async Task<IActionResult> Details(int id)
    {
        var employee = await employeeService.FindById(id);
        return View(employee);
    }
}