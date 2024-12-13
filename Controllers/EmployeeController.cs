using DHR.Helper;
using DHR.Models;
using DHR.Providers;
using DHR.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DHR.Controllers;

[Authorize(Roles = "Admin")]
public class EmployeeController(
    AppDbContext context,
    UserManager<Users> userManager,
    MongoDbContext mongoDbContext,
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
        ViewBag.Taxes = await context.TaxExemptIncomes.ToListAsync();
        ViewBag.SubUnits = await context.SubUnits.Include(x => x.Unit).ToListAsync();
        ViewBag.Religions = await context.Religions.ToListAsync();
        ViewBag.Educations = await context.Educations.ToListAsync();

        return View();
    }

    // POST: EmployeeController/Create
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(CreateEmployeeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Divisions = await divisionService.FindAll();
            ViewBag.JobTitle = await jobTitleService.FindAll();
            ViewBag.Taxes = await context.TaxExemptIncomes.ToListAsync();
            ViewBag.SubUnits = await context.SubUnits.Include(x => x.Unit).ToListAsync();
            ViewBag.Religions = await context.Religions.ToListAsync();
            ViewBag.Educations = await context.Educations.ToListAsync();
            return View(model);
        }

        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Logout", "Account");
        }
        var timestamp = DateTime.UtcNow;
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            // Buat pengguna
            var createUser = await userManager.CreateAsync(new Users
            {
                UserName = GenerateUsername(model.Email),
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = true,
                EmailConfirmed = true
            }, model.Nip);
            //asign role
            
            if (!createUser.Succeeded)
            {
                var errors = string.Join(", ", createUser.Errors.Select(x => x.Description));
                TempData["Errors"] = errors;
                await transaction.RollbackAsync();
                return RedirectToAction(nameof(Create));
            }

            var newUser = await userManager.FindByEmailAsync(model.Email);
            if (newUser == null)
            {
                TempData["Errors"] = "User not found";
                await transaction.RollbackAsync();
                return RedirectToAction(nameof(Create));
            }
            await userManager.AddToRoleAsync(newUser, "User");
            await context.Employee.AddAsync(new EmployeeModel
            {
                Nip = int.Parse(model.Nip),
                Nik = model.Nik,
                Npwp = model.Npwp,
                Gender = model.Gender,
                Address = model.Address,
                JoinDate = model.JoinDate,
                DivisionId = model.DivisionId,
                JobTitleId = model.JobTitleId,
                ReligionId = model.ReligionId,
                EducationId = model.EducationId,
                TaxExemptIncomeId = model.TaxExemptIncomeId,
                SubUnitId = model.SubUnitId,
                UserId = newUser.Id,
                CreatedBy = user.Id,
                CreatedAt = timestamp,
                UpdatedBy = user.Id,
                UpdatedAt = timestamp
            });
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            TempData["Success"] = $"Employee: {newUser.FullName} has been created";
            var logs = new AppLogModel
            {
                CreatedAt = timestamp,
                CreatedBy = $"{user.Id} - {user.FullName}",
                Params = JsonConvert.SerializeObject(model),
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "EmployeeController",
                    Action = "Create",
                    Database = "Employee & AspNetUsers"
                }),
            };
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            TempData["Errors"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }


    private string GenerateUsername(string email)
    {
        var username = email.Split('@')[0];
        return username;
    }
}