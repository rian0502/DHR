using DHR.Helper;
using DHR.Models;
using DHR.ViewModels.EmployeeBenefit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DHR.Controllers;

[Authorize(Roles = "Admin")]
public class EmployeeBenefitController(
    AppDbContext context,
    MongoDbContext mongoDbContext,
    UserManager<Users> userManager) : Controller
{
    public async Task<IActionResult> Details(int id)
    {
        var employee = await context.Employee
            .Where(e => e.EmployeeId == id)
            .Include(e => e.Benefits)!
            .ThenInclude(b => b.Benefit)
            .Include(e => e.Users)
            .FirstOrDefaultAsync();

        return View(employee);
    }

    public async Task<IActionResult> Create(int id)
    {
        ViewBag.Benefits = await context.Benefits.ToListAsync();
        return View(new CreateEmployeeBenefit
        {
            EmployeeId = id
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEmployeeBenefit model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Benefits = await context.Benefits.ToListAsync();
                return View(new CreateEmployeeBenefit
                {
                    EmployeeId = model.EmployeeId
                });
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }
            var time = DateTime.UtcNow;
            await context.EmployeeBenefits.AddAsync(new EmployeeBenefit
            {
                EmployeeId = model.EmployeeId,
                BenefitId = model.BenefitId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Amount = model.Amount
            });
            await context.SaveChangesAsync();
            var logs = new AppLogModel
            {
                CreatedAt = time,
                CreatedBy = $"{user.Id} - {user.FullName}",
                Params = JsonConvert.SerializeObject(model),
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "EmployeeBenefitController",
                    Action = "Create",
                    Database = "EmployeeBenefits"
                })
            };
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Benefit added successfully";
            return RedirectToAction("Details", "EmployeeBenefit", new { id = model.EmployeeId });
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return RedirectToAction("Create", new { id = model.EmployeeId });
        }
    }

    public IActionResult Edit(int id)
    {
        return View();
    }
}