using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.Providers;
using DAHAR.ViewModels.SubUnit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class SubUnitController(
    SubUnitService subUnitService,
    WorkAreaService workAreaService,
    UnitService unitService,
    UserManager<Users> userManager,
    MongoDBContext mongoDbContext) : Controller
{
    // GET: SubUnitController
    public async Task<ActionResult> Index()
    {
        var subUnits = await subUnitService.FindAll();
        return View(subUnits);
    }

    public async Task<ActionResult> Create()
    {
        ViewBag.WorkAreas = await workAreaService.FindAll();
        ViewBag.Units = await unitService.FindAll();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateSubUnitViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.WorkAreas = await workAreaService.FindAll();
                ViewBag.Units = await unitService.FindAll();
                return View(model);
            }
            else
            {
                var users = await userManager.GetUserAsync(User);
                var currentTime = DateTime.UtcNow;
                var insert = await subUnitService.Insert(model, users!.Id, currentTime);

                if (insert == 3)
                {
                    TempData["Errors"] = "Sub Unit Code or Sub Unit Name already exists!";
                    return RedirectToAction(nameof(Create));
                }

                var logs = new AppLogModel
                {
                    CreatedAt = currentTime,
                    CreatedBy = $"{users.Id} - {users.FullName}",
                    Params = JsonConvert.SerializeObject(new
                    {
                        model.SubUnitCode,
                        model.SubUnitName,
                        model.SubUnitAddress,
                        model.UnitId,
                        model.LocationId
                    }),
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "SubUnitController",
                        Action = "Create",
                        Database = "SubUnit"
                    }),
                };
                await mongoDbContext.AppLogs.InsertOneAsync(logs);
                TempData["Success"] = "Sub Unit has been created successfully!";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return RedirectToAction(nameof(Create));
        }
    }

    public async Task<ActionResult> Edit(int id)
    {
        ViewBag.WorkAreas = await workAreaService.FindAll();
        ViewBag.Units = await unitService.FindAll();
        var subUnit = await subUnitService.FindById(id);
        return View(new EditSubUnitViewModel
        {
            SubUnitId = subUnit.SubUnitID,
            SubUnitCode = subUnit.SubUnitCode,
            SubUnitName = subUnit.SubUnitName,
            SubUnitAddress = subUnit.SubUnitAddress,
            UnitId = subUnit.UnitID,
            LocationId = subUnit.LocationID,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, EditSubUnitViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.WorkAreas = await workAreaService.FindAll();
                ViewBag.Units = await unitService.FindAll();
                return View(model);
            }

            var oldData = await subUnitService.FindById(id);
            var users = await userManager.GetUserAsync(User);
            var currentTime = DateTime.UtcNow;
            var update = await subUnitService.Update(model, users!.Id, currentTime);
            if (update == 0)
            {
                TempData["Errors"] = "Something went wrong!";
                return RedirectToAction(nameof(Edit), new { id });
            }
            var logs = new AppLogModel
            {
                CreatedAt = currentTime,
                CreatedBy = $"{users.Id} - {users.FullName}",
                Params = JsonConvert.SerializeObject(new
                {
                    oldData = JsonConvert.SerializeObject(new
                    {
                        oldData.SubUnitID,
                        oldData.SubUnitCode,
                        oldData.SubUnitAddress,
                        oldData.SubUnitName,
                        oldData.LocationID,
                        oldData.Location!.LocationName,
                        oldData.UnitID,
                        oldData.Unit!.UnitName,
                        oldData.CreatedAt,
                        oldData.CreatedBy,
                        oldData.UpdatedAt,
                        oldData.UpdatedBy
                    }),
                    newData = JsonConvert.SerializeObject(new
                    {
                        model.SubUnitId,
                        model.SubUnitCode,
                        model.SubUnitAddress,
                        model.SubUnitName,
                        model.LocationId,
                        model.UnitId,
                    }),
                }),
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "SubUnitController",
                    Action = "Edit",
                    Database = "SubUnit"
                }),
            };
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Sub Unit has been updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return RedirectToAction(nameof(Create));
        }
    }
}