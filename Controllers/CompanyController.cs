using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.Providers;
using DAHAR.ViewModels.Company;
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
    MongoDbContext mongoDbContext)
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
                            LocationID = model.LocationId
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
        return View(new EditCompanyViewModel
        {
            CompanyId = result.CompanyId,
            CompanyCode = result.CompanyCode ?? "",
            CompanyName = result.CompanyName ?? "",
            LocationId = result.LocationId
        });
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
                        oldData.CompanyId,
                        oldData.CompanyCode,
                        oldData.CompanyName,
                        oldData.LocationId,
                        oldData.CreatedAt,
                        oldData.CreatedBy,
                        oldData.UpdatedAt,
                        oldData.UpdatedBy
                    }),
                    newData = JsonConvert.SerializeObject(new
                    {
                        CompanyID = model.CompanyId,
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