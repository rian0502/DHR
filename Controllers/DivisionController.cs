using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.Providers;
using DAHAR.ViewModels.Divison;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class DivisionController(
    DivisionService divisionService,
    UserManager<Users> userManager,
    MongoDBContext mongoDbContext,
    SubDepartmentService subDepartmentService)
    : Controller
{
    // GET: DivisionController
    public async Task<ActionResult> Index()
    {
        var divisions = await divisionService.FindAll();
        return View(divisions);
    }

    // GET: DivisionController/Create
    public async Task<ActionResult> Create()
    {
        ViewBag.SubDepartments = await subDepartmentService.FindAll();
        return View();
    }

    // POST: DivisionController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateDivisionViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubDepartments = await subDepartmentService.FindAll();
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            var time = DateTime.UtcNow;
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var insert = await divisionService.Insert(model, user.Id, time);
            switch (insert)
            {
                case 0:
                    TempData["Errors"] = "Failed to create division";
                    break;
                case 3:
                    TempData["Errors"] = "Division already exists";
                    break;
                default:
                    var logs = new AppLogModel
                    {
                        CreatedBy = $"{user.Id} - {user.FullName}",
                        CreatedAt = time,
                        Params = JsonConvert.SerializeObject(new
                        {
                            model.DivisionCode,
                            model.DivisionName,
                            model.SubDepartmentId
                        }),
                        Source = JsonConvert.SerializeObject(new
                        {
                            Controller = "DivisionController",
                            Action = "Create",
                            Database = "Divisions"
                        })
                    };
                    await mongoDbContext.AppLogs.InsertOneAsync(logs);
                    TempData["Success"] = "Division created successfully";
                    break;
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: DivisionController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        ViewBag.SubDepartments = await subDepartmentService.FindAll();
        var division = await divisionService.FindById(id);
        return View(new EditDivisionViewModel
        {
            DivisionId = division.DivisionId,
            DivisionCode = division.DivisionCode ?? "",
            DivisionName = division.DivisionName ?? "",
            SubDepartmentId = division.SubDepartmentId
        });
    }

    // POST: DivisionController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, EditDivisionViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubDepartments = await subDepartmentService.FindAll();
                return View(model);
            }

            var oldData = await divisionService.FindById(id);
            var user = await userManager.GetUserAsync(User);
            var time = DateTime.UtcNow;
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var update = await divisionService.Update(model, user.Id, time);
            if (update == 0)
            {
                TempData["Errors"] = "Failed to update division";
            }
            else
            {
                var logs = new AppLogModel
                {
                    CreatedAt = time,
                    CreatedBy = $"{user.Id} - {user.FullName}",
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "DivisionController",
                        Action = "Edit",
                        Database = "Divisions"
                    }),
                    Params = JsonConvert.SerializeObject(new
                    {
                        oldData = JsonConvert.SerializeObject(new
                        {
                            oldData.DivisionId,
                            oldData.DivisionCode,
                            oldData.DivisionName,
                            oldData.SubDepartmentId,
                            oldData.CreatedAt,
                            oldData.CreatedBy,
                            oldData.UpdatedAt,
                            oldData.UpdatedBy
                        }),
                        newData = JsonConvert.SerializeObject(new
                        {
                            model.DivisionId,
                            model.DivisionCode,
                            model.DivisionName,
                            model.SubDepartmentId
                        })
                    })
                };
                await mongoDbContext.AppLogs.InsertOneAsync(logs);
                TempData["Success"] = "Division updated successfully";
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}