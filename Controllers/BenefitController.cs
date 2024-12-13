using DHR.Helper;
using DHR.Models;
using DHR.ViewModels.Benefit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DHR.Controllers;

[Authorize(Roles = "Admin")]
public class BenefitController(
    AppDbContext context,
    MongoDbContext mongoDbContext,
    UserManager<Users> userManager)
    : Controller
{
    // GET: BenefitController
    public async Task<ActionResult> Index()
    {
        var benefits = await EntityFrameworkQueryableExtensions.ToListAsync(context.Benefits);
        return View(benefits);
    }

    // GET: BenefitController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: BenefitController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateBenefitViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                var time = DateTime.UtcNow;

                var benefit = new BenefitModel
                {
                    BenefitName = model.BenefitName,
                    BenefitDescription = model.BenefitDescription,
                    IsMonetary = model.IsMonetary,
                    IsActive = model.IsActive,
                    Category = model.Category,
                    CreatedBy = user.Id,
                    CreatedAt = time,
                    UpdatedBy = user.Id,
                    UpdatedAt = time
                };
                await context.Benefits.AddAsync(benefit);
                await context.SaveChangesAsync();
                var logs = new AppLogModel
                {
                    CreatedAt = time,
                    CreatedBy = $"{user.Id} - {user.FullName}",
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "BenefitController",
                        Action = "Create",
                        Database = "Benefits"
                    }),
                    Params = JsonConvert.SerializeObject(model)
                };
                await mongoDbContext.AppLogs.InsertOneAsync(logs);
                TempData["Success"] = "Benefit has been created successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        catch (Exception exception)
        {
            TempData["Errors"] = exception.Message;
            return View(model);
        }
    }

    // GET: BenefitController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        var model = await context.Benefits.FindAsync(id);
        if (model == null)
        {
            TempData["Errors"] = "Benefit not found";
            return RedirectToAction(nameof(Index));
        }

        return View(new EditBenefitViewModel
        {
            BenefitId = model.BenefitId,
            BenefitName = model.BenefitName ?? "",
            BenefitDescription = model.BenefitDescription ?? "",
            IsMonetary = model.IsMonetary,
            IsActive = model.IsActive
        });
    }

    // POST: BenefitController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, EditBenefitViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var time = DateTime.UtcNow;
            var oldData = await context.Benefits.FindAsync(id);
            if (oldData == null)
            {
                TempData["Errors"] = "Benefit not found";
                return RedirectToAction(nameof(Index));
            }

            var logs = new AppLogModel
            {
                CreatedAt = time,
                CreatedBy = $"{user.Id} - {user.FullName}",
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "BenefitController",
                    Action = "Edit",
                    Database = "Benefits"
                }),
                Params = JsonConvert.SerializeObject(new
                {
                    oldData = JsonConvert.SerializeObject(oldData),
                    newData = JsonConvert.SerializeObject(model)
                })
            };
            oldData.BenefitName = model.BenefitName;
            oldData.BenefitDescription = model.BenefitDescription;
            oldData.IsMonetary = model.IsMonetary;
            oldData.IsActive = model.IsActive;
            oldData.Category = model.Category;
            oldData.UpdatedAt = time;
            oldData.UpdatedBy = user.Id;
            await context.SaveChangesAsync();
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Benefit has been updated successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception exception)
        {
            TempData["Errors"] = exception.Message;
            return View(model);
        }
    }

    // POST: BenefitController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id, IFormCollection collection)
    {
        try
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var model = await context.Benefits
                .Include(b => b.EmployeeBenefits)
                .FirstOrDefaultAsync(b => b.BenefitId == id);

            if (model == null)
            {
                TempData["Errors"] = "Benefit not found";
                return RedirectToAction(nameof(Index));
            }

            if (model.EmployeeBenefits is { Count: > 0 })
            {
                TempData["Errors"] = "Benefit cannot be deleted because it is being used by employees";
            }

            var logs = new AppLogModel
            {
                CreatedAt = DateTime.UtcNow,
                CreatedBy = $"{user.Id} - {user.FullName}",
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "BenefitController",
                    Action = "Delete",
                    Database = "Benefits"
                }),
                Params = JsonConvert.SerializeObject(new
                {
                    model.BenefitId,
                    model.BenefitName,
                    model.Category,
                    model.BenefitDescription,
                    model.IsActive,
                    model.IsMonetary,
                    model.CreatedBy,
                    model.CreatedAt,
                    model.UpdatedBy,
                    model.UpdatedAt
                })
            };
            context.Benefits.Remove(model);
            await context.SaveChangesAsync();
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Benefit has been deleted successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception exception)
        {
            TempData["Errors"] = exception.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}