using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.Providers;
using DAHAR.ViewModels.Period;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class PeriodsController(
    PeriodService periodService,
    AppDbContext context,
    UserManager<Users> userManager,
    MongoDbContext mongoDbContext)
    : Controller
{
    // GET: PeriodsController
    public async Task<ActionResult> Index()
    {
        var periods = await periodService.FindAll();
        return View(periods);
    }


    // GET: PeriodsController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: PeriodsController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreatePeriodViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            var time = DateTime.UtcNow;
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var checkPeriod = await context.Periods.FirstOrDefaultAsync(x =>
                x.StartPeriodDate == model.StartPeriodDate || x.EndPeriodDate == model.EndPeriodDate);
            if (checkPeriod != null)
            {
                TempData["Errors"] = "Period already exists";
                return View(model);
            }

            if (model.IsActive)
            {
                var activePeriod = await context.Periods.FirstOrDefaultAsync(x => x.IsActive);
                if (activePeriod != null)
                {
                    activePeriod.IsActive = false;
                    context.Periods.Update(activePeriod);
                    await context.SaveChangesAsync();
                }
            }

            var period = new PeriodModel()
            {
                StartPeriodDate = model.StartPeriodDate,
                EndPeriodDate = model.EndPeriodDate,
                IsActive = model.IsActive,
                CreatedAt = time,
                CreatedBy = user.Id,
                UpdatedAt = time,
                UpdatedBy = user.Id
            };
            await context.Periods.AddAsync(period);
            await context.SaveChangesAsync();
            var logs = new AppLogModel
            {
                CreatedAt = time,
                CreatedBy = $"{user.Id} - {user.FullName}",
                Params = JsonConvert.SerializeObject(new
                {
                    model.StartPeriodDate,
                    model.EndPeriodDate,
                    model.IsActive
                }),
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "PeriodsController",
                    Action = "Create",
                    Database = "Periods",
                })
            };
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Period has been created";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return View(model);
        }
    }

    // GET: PeriodsController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        var period = await context.Periods.FindAsync(id);
        if (period != null)
        {
            return View(new EditPeriodViewModel
            {
                PeriodId = period.PeriodId,
                StartPeriodDate = period.StartPeriodDate,
                EndPeriodDate = period.EndPeriodDate,
                IsActive = period.IsActive
            });
        }

        TempData["Errors"] = "Period not found";
        return RedirectToAction(nameof(Index));
    }

    // POST: PeriodsController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, EditPeriodViewModel model)
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

        var oldData = await context.Periods.FindAsync(id);
        if (oldData == null)
        {
            TempData["Errors"] = "Period not found";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var time = DateTime.UtcNow;

            // Jika data aktif diubah menjadi aktif, nonaktifkan period aktif lainnya
            if (model.IsActive && !oldData.IsActive)
            {
                var activePeriod = await context.Periods.FirstOrDefaultAsync(x => x.IsActive && x.PeriodId != id);
                if (activePeriod != null)
                {
                    activePeriod.IsActive = false;
                }
            }
            
            oldData.StartPeriodDate = model.StartPeriodDate;
            oldData.EndPeriodDate = model.EndPeriodDate;
            oldData.IsActive = model.IsActive;
            oldData.UpdatedAt = time;
            oldData.UpdatedBy = user.Id;

            var logs = new AppLogModel
            {
                CreatedAt = time,
                CreatedBy = $"{user.Id} - {user.FullName}",
                Params = JsonConvert.SerializeObject(new
                {
                    oldData = JsonConvert.SerializeObject(new
                    {
                        oldData.PeriodId,
                        oldData.StartPeriodDate,
                        oldData.EndPeriodDate,
                        oldData.IsActive,
                        oldData.CreatedBy,
                        oldData.CreatedAt,
                        oldData.UpdatedAt,
                        oldData.UpdatedBy
                    }),
                    newData = model
                }),
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "PeriodsController",
                    Action = "Edit",
                    Database = "Periods"
                })
            };

            context.Periods.Update(oldData);
            await context.SaveChangesAsync();
            await mongoDbContext.AppLogs.InsertOneAsync(logs);

            TempData["Success"] = "Period has been updated";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return View(model);
        }
    }


    // POST: PeriodsController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return RedirectToAction(nameof(Index));
        }
    }
}