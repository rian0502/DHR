using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.Providers;
using DAHAR.ViewModels.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class DepartmentController(
    DepartmentService departmentService,
    CompanyService companyService,
    UserManager<Users> userManager,
    MongoDbContext mongoDbContext) : Controller
{
    public async Task<IActionResult> Index()
    {
        var departments = await departmentService.FindAll();
        return View(departments);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Companies = await companyService.FindAll();
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(CreateDepartmentViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Companies = await companyService.FindAll();
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var time = DateTime.UtcNow;
            var insert = await departmentService.Insert(model, user.Id, time);
            switch (insert)
            {
                case 0:
                    TempData["Errors"] = "Failed to create department";
                    break;
                case 3:
                    TempData["Errors"] = "Department already exists";
                    break;
                default:
                    var logs = new AppLogModel
                    {
                        CreatedAt = time,
                        CreatedBy = $"{user.Id} - {user.FullName}",
                        Params = JsonConvert.SerializeObject(new
                        {
                            Department = model
                        }),
                        Source = JsonConvert.SerializeObject(new
                        {
                            Controller = "DepartmentController",
                            Action = "Create",
                            Database = "Departments",
                        })
                    };
                    await mongoDbContext.AppLogs.InsertOneAsync(logs);
                    TempData["Success"] = "Department created successfully";
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
        ViewBag.Companies = await companyService.FindAll();
        var department = await departmentService.FindById(id);
        return View(new EditDepartmentViewModel
        {
            DepartmentId = department.DepartmentId,
            DepartmentCode = department.DepartmentCode ?? "",
            DepartmentName = department.DepartmentName ?? "",
            CompanyId = department.CompanyId ?? 0
        });
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Edit(int id, EditDepartmentViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Companies = await companyService.FindAll();
                return View(model);
            }

            var oldData = await departmentService.FindById(id);
            var user = await userManager.GetUserAsync(User);
            var time = DateTime.UtcNow;
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }
            var update = await departmentService.Update(model, user.Id, time);
            if (update == 0)
            {
                TempData["Errors"] = "Failed to update department";
            }
            else
            {
                var logs = new AppLogModel
                {
                    CreatedAt = time,
                    CreatedBy = $"{user.Id} - {user.FullName}",
                    Params = JsonConvert.SerializeObject(new
                    {
                        oldData = JsonConvert.SerializeObject(new
                        {
                            oldData.DepartmentId,
                            oldData.DepartmentCode,
                            oldData.DepartmentName,
                            oldData.CompanyId,
                            oldData.CreatedAt,
                            oldData.CreatedBy,
                            oldData.UpdatedAt,
                            oldData.UpdatedBy
                        }),
                        newData = JsonConvert.SerializeObject(model)
                    }),
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "DepartmentController",
                        Action = "Edit",
                        Database = "Departments",
                    })
                };
                await mongoDbContext.AppLogs.InsertOneAsync(logs);
                TempData["Success"] = "Department updated successfully";
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