using DAHAR.Helper;
using DAHAR.Providers;
using DAHAR.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class EmployeeController(
    AppDBContext context,
    EmployeeService employeeService,
    DivisionService divisionService,
    JobTitleService jobTitleService
)
    : Controller
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

    //GET: EmployeeController/Create

    public async Task<IActionResult> Create()
    {
        ViewBag.Divisions = await divisionService.FindAll();
        ViewBag.JobTitle = await jobTitleService.FindAll();
        return View();
    }

    //POST: EmployeeController/Create
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(CreateEmployeeViewModel model)
    {
        if (model.Nip != null)
        {
            var nip = await context.Employee.FirstOrDefaultAsync(x => x.EmployeeID == int.Parse(model.Nip));
            if (nip != null)
            {
                ModelState.AddModelError("Nip", "NIP already exist");
            }
        }

        if (model.Nik != null)
        {
            var nik = await context.Employee.FirstOrDefaultAsync(x => x.EmployeeID == int.Parse(model.Nik));
            if (nik != null)
            {
                ModelState.AddModelError("Nik", "NIK already exist");
            }
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Divisions = await divisionService.FindAll();
            ViewBag.JobTitle = await jobTitleService.FindAll();
            return View(model);
        }

        return View(model);
    }
}