using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.Providers;
using DAHAR.ViewModels.WorkArea;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class WorkAreaController(
    WorkAreaService workAreaService,
    MongoDBContext mongoDbContext,
    UserManager<Users> userManager)
    : Controller
{
    public async Task<IActionResult> Index()
    {
        var locations = await workAreaService.FindAll();
        return View(locations);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateWorkAreaViewModel model)
    {
        if (ModelState.IsValid)
        {
            var currentTime = DateTime.UtcNow;
            var user = await userManager.GetUserAsync(User);
            var insert = await workAreaService.Create(model, user!.Id, currentTime);
            if (insert == 0)
            {
                TempData["Errors"] = "Failed to create Work Area";
                return View(model);
            }
            else
            {
                var logs = new AppLogModel()
                {
                    Params = JsonConvert.SerializeObject(new
                    {
                        model.LocationCode,
                        model.LocationName,
                    }),
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "WorkAreaController",
                        Action = "Create",
                        Database = "Locations"
                    }),
                    CreatedBy = $"{user!.Id} - {user.FullName}",
                    CreatedAt = DateTime.UtcNow
                };
                await mongoDbContext.AppLogs.InsertOneAsync(logs);

                TempData["Success"] = "Work Area has ben created";
                return RedirectToAction("Index");
            }
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await workAreaService.FindById(id);
        EditWorkAreaViewModel model = new()
        {
            LocationID = result.LocationID,
            LocationCode = result.LocationCode,
            LocationName = result.LocationName
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditWorkAreaViewModel model)
    {
        if (ModelState.IsValid)
        {
            var currentTime = DateTime.UtcNow;
            var user = await userManager.GetUserAsync(User);
            var oldData = await workAreaService.FindById(id);
            var result = await workAreaService.Update(model: model, userId: user.Id, time: currentTime);
            if (result == 0)
            {
                TempData["Errors"] = "Failed to create Work Area";
                return View(model);
            }
            else
            {
                var logs = new AppLogModel()
                {
                    Params = JsonConvert.SerializeObject(new
                    {
                        oldData,
                        newData = model
                    }),
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "WorkAreaController",
                        Action = "Edit",
                        Database = "Locations"
                    }),
                    CreatedBy = $"{user!.Id} - {user.FullName}",
                    CreatedAt = DateTime.UtcNow
                };
                await mongoDbContext.AppLogs.InsertOneAsync(logs);
                TempData["Success"] = "Work Area has ben updated";
                return RedirectToAction("Index");
            }
        }

        return View(model);
    }
}