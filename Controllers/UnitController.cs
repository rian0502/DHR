using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.Providers;
using DAHAR.ViewModels.Unit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class UnitController(UnitService unitService, UserManager<Users> userManager, MongoDBContext mongoDbContext)
    : Controller
{
    // GET: UnitController
    public async Task<ActionResult> Index()
    {
        var units = await unitService.FindAll();
        return View(units);
    }

    // GET: UnitController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: UnitController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateUnitViewModel model)
    {
        try
        {
            if (!ModelState.IsValid) return View(model);
            var user = await userManager.GetUserAsync(User);
            var currentTime = DateTime.UtcNow;
            var insert = await unitService.Insert(model, user!.Id, currentTime);
            switch (insert)
            {
                case 3:
                    TempData["Errors"] = "Unit Already Exist";
                    return View(model);
                case 0:
                    TempData["Errors"] = "Something Went Wrong";
                    return View(model);
                default:
                {
                    var logs = new AppLogModel()
                    {
                        CreatedBy = $"{user.Id} - {user.FullName}",
                        CreatedAt = currentTime,
                        Params = JsonConvert.SerializeObject(new
                        {
                            model.UnitCode,
                            model.UnitName
                        }),
                        Source =JsonConvert.SerializeObject(new
                        {
                            Controller = "UnitController",
                            Action = "Create",
                            Database = "Units"
                        }),
                    };
                    await mongoDbContext.AppLogs.InsertOneAsync(logs);
                    TempData["Success"] = "Unit Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return View(model);
        }
    }

    // GET: UnitController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        var unit = await unitService.FindById(id);
        return View(new EditUnitViewModel
        {
            UnitId = unit.UnitID,
            UnitCode = unit.UnitCode,
            UnitName = unit.UnitName
        });
    }

    // POST: UnitController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, EditUnitViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var oldData = await unitService.FindById(id);
            var user = await userManager.GetUserAsync(User);
            var currentTime = DateTime.UtcNow;
            var update = await unitService.Update(model, user!.Id, currentTime);
            if (update == 0)
            {
                TempData["Errors"] = "Failed to Update Unit";
            }
            else
            {
                var logs = new AppLogModel()
                {
                    CreatedBy = $"{user.Id} - {user.FullName}",
                    CreatedAt = currentTime,
                    Params = JsonConvert.SerializeObject(new
                    {
                        OldData = oldData,
                        NewData = model
                    }),
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "UnitController",
                        Action = "Edit",
                        Database = "Units"
                    })
                };
                await mongoDbContext.AppLogs.InsertOneAsync(logs);
                TempData["Success"] = "Unit Updated Successfully";
            }
            return RedirectToAction(nameof(Index));
        }
        catch(Exception e)
        {
            TempData["Errors"] = e.Message;
            return View(model);
        }
    }

    // GET: UnitController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: UnitController/Delete/5
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
            return View();
        }
    }
}