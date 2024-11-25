using DAHAR.Helper;
using DAHAR.Models;
using Newtonsoft.Json;
using DAHAR.Providers;
using DAHAR.ViewModels.SubDepartment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class SubDepartmentController(
    SubDepartmentService subDepartmentService,
    DepartmentService departmentService,
    UserManager<Users> userManager,
    MongoDBContext mongoDbContext)
    : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var subDepartments = await subDepartmentService.FindAll();
        return View(subDepartments);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Departments = await departmentService.FindAll();
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(CreateSubDepartmentViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await departmentService.FindAll();
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            var time = DateTime.UtcNow;
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var insert = await subDepartmentService.Insert(model, user.Id, time);
            switch (insert)
            {
                case 3:
                    TempData["Errors"] = "Sub Department Code already exists";
                    break;
                case 0:
                    TempData["Errors"] = "Failed to create sub department";
                    break;
                default:
                    var logs = new AppLogModel
                    {
                        CreatedAt = time,
                        CreatedBy = $"{user.Id} - {user.FullName}",
                        Params = JsonConvert.SerializeObject(new
                        {
                            SubDepartment = model
                        }),
                        Source = JsonConvert.SerializeObject(new
                        {
                            Controller = "SubDepartmentController",
                            Action = "Create",
                            Database = "SubDepartments",
                        }),
                    };
                    await mongoDbContext.AppLogs.InsertOneAsync(logs);
                    TempData["Success"] = "Sub Department created successfully";
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
        ViewBag.Departments = await departmentService.FindAll();
        var subDepartment = await subDepartmentService.FindById(id);
        return View(new EditSubDepartmentViewModel
        {
            SubDepartmentId = subDepartment.SubDepartmentID,
            SubDepartmentCode = subDepartment.SubDepartmentCode ?? "",
            SubDepartmentName = subDepartment.SubDepartmentName ?? "",
            DepartmentId = subDepartment.DepartmentID
        });
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Edit(int id, EditSubDepartmentViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await departmentService.FindAll();
                return View(model);
            }

            var oldData = await subDepartmentService.FindById(id);
            var time = DateTime.UtcNow;
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var update = await subDepartmentService.Update(model, user.Id, time);
            if (update == 0)
            {
                TempData["Errors"] = "Failed to update data Sub-department";
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
                            oldData.SubDepartmentID,
                            oldData.SubDepartmentCode,
                            oldData.SubDepartmentName,
                            oldData.DepartmentID,
                            oldData.CreatedAt,
                            oldData.CreatedBy,
                            oldData.UpdatedAt,
                            oldData.UpdatedBy
                        }),
                        newData = JsonConvert.SerializeObject(new
                        {
                            model.SubDepartmentId,
                            model.SubDepartmentCode,
                            model.SubDepartmentName,
                            model.DepartmentId
                        })
                    }),
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "SubDepartmentController",
                        Action = "Edit",
                        Database = "SubDepartments",
                    }),
                };
                await mongoDbContext.AppLogs.InsertOneAsync(logs);
                TempData["Success"] = "Sub-department updated successfully";
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