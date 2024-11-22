using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.Providers;
using DAHAR.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class CompanyController(
    CompanyService companyService,
    WorkAreaService workAreaService,
    UserManager<Users> userManager,
    MongoDBContext mongoDbContext)
    : Controller
{
    public async Task<IActionResult> Index()
    {
        var companies = await companyService.FindAll();
        return View(companies);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.workAreas = await workAreaService.FindAll();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCompanyViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.WorkAreas = await workAreaService.FindAll();
                return View(model);
            }
            var user = await userManager.GetUserAsync(User);
            var currentTime = DateTime.UtcNow;
            var result = await companyService.Insert(model, user!.Id, currentTime);
            switch (result)
            {
                case 3:
                    TempData["Errors"] = "Company Code already exists";
                    break;
                case 0:
                    TempData["Errors"] = "Failed to create company";
                    break;
                default:
                    var logs = new AppLogModel
                    {
                        CreatedBy = $"{user.Id} - {user.FullName}",
                        CreatedAt = currentTime,
                        Params = JsonConvert.SerializeObject(new
                        {
                            model.CompanyCode,
                            model.CompanyName,
                            model.LocationID
                        }),
                        Source = JsonConvert.SerializeObject(new
                        {
                            Controller = "CompanyController",
                            Action = "Create",
                            Database = "Companies"
                        })
                    };
                    await mongoDbContext.AppLogs.InsertOneAsync(logs);
                    TempData["Success"] = "Company created successfully";
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

    public async Task<IActionResult> Edit(int id)
    {
        var result = await companyService.FindById(id);
        ViewBag.WorkAreas = await workAreaService.FindAll();
        EditCompanyViewModel model = new EditCompanyViewModel
        {
            CompanyID = result.CompanyID,
            CompanyCode = result.CompanyCode,
            CompanyName = result.CompanyName,
            LocationID = result.LocationID,
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditCompanyViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.WorkAreas = await workAreaService.FindAll();
            return View(model);
        }
        var oldData = await companyService.FindById(id);
        var user = await userManager.GetUserAsync(User);
        var currentTime = DateTime.UtcNow;
        var result = await companyService.Update(model, user!.Id, currentTime);
        if (result != 0)
        {
            var logs = new AppLogModel
            {
                CreatedBy = $"{user.Id} - {user.FullName}",
                CreatedAt = currentTime,
                Params = JsonConvert.SerializeObject(new
                {
                    oldData = JsonConvert.SerializeObject(new
                    {
                        oldData.CompanyID,
                        oldData.CompanyCode,
                        oldData.CompanyName,
                        oldData.LocationID,
                        oldData.CreatedAt,
                        oldData.CreatedBy,
                        oldData.UpdatedAt,
                        oldData.UpdatedBy
                    }),
                    newData = JsonConvert.SerializeObject(new
                    {
                        model.CompanyID,
                        model.CompanyCode,
                        model.CompanyName
                    })
                }),
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "CompanyController",
                    Action = "Edit",
                    Database = "Companies"
                })
            };
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Company updated successfully";
        }
        else
        {
            TempData["Errors"] = "Failed to update company";
        }

        return RedirectToAction("Index");
    }
}