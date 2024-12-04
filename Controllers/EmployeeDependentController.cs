using DAHAR.Helper;
using DAHAR.Models;
using Microsoft.AspNetCore.Mvc;
using DAHAR.ViewModels.EmployeeDependent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DAHAR.Controllers;

[Authorize(Roles = "User")]
public class EmployeeDependentController(
    AppDbContext context,
    UserManager<Users> userManager,
    MongoDbContext mongoDbContext
) : Controller
{
    // GET: EmployeeDependentController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: EmployeeDependentController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateDepentdentViewModel model)
    {
        try
        {
            var user = await userManager.GetUserAsync(User);
            var time = DateTime.UtcNow;
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var employee = await context.Employee
                .Include(x => x.EmployeeDependents)
                .FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (employee == null)
            {
                TempData["Errors"] = "Employee not found";
                return RedirectToAction("Index", "Profile");
            }

            var insert = await context.EmployeeDependents.AddAsync(new EmployeeDependentModel
            {
                EmployeeId = employee.EmployeeId,
                DependentName = model.DependentName,
                DependentGender = model.DependentGender,
                DependentStatus = model.DependentStatus,
                CreatedBy = user.Id,
                CreatedAt = time,
                UpdatedBy = user.Id,
                UpdatedAt = time
            });
            await context.SaveChangesAsync();
            var logs = new AppLogModel
            {
                CreatedBy = $"{user.Id} - {user.FullName}",
                CreatedAt = time,
                Params = JsonConvert.SerializeObject(new
                {
                    newData = JsonConvert.SerializeObject(new
                    {
                        insert.Entity.EmployeeDependentId,
                        insert.Entity.DependentName,
                        insert.Entity.DependentGender,
                        insert.Entity.DependentStatus
                    })
                }),
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "EmployeeDependentController",
                    Action = "Create",
                    Database = "EmployeeDependents",
                })
            };
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Dependent added successfully";
            return RedirectToAction("Index", "Profile");
        }
        catch
        {
            return RedirectToAction("Index", "Profile");
        }
    }

    // GET: EmployeeDependentController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        var dependent = await context.EmployeeDependents.FindAsync(id);
        if (dependent != null)
        {
            return View(new EditDepentdentViewModel
            {
                EmployeeDependentId = dependent.EmployeeDependentId,
                DependentName = dependent.DependentName ?? "",
                DependentGender = dependent.DependentGender ?? "",
                DependentStatus = dependent.DependentStatus ?? ""
            });
        }

        TempData["Errors"] = "Dependent not found";
        return RedirectToAction("Index", "Profile");
    }

    // POST: EmployeeDependentController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, EditDepentdentViewModel model)
    {
        try
        {
            var user = await userManager.GetUserAsync(User);
            var time = DateTime.UtcNow;
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var dependent = await context.EmployeeDependents.FindAsync(id);
            if (dependent == null)
            {
                TempData["Errors"] = "Dependent not found";
                return RedirectToAction("Index", "Profile");
            }

            //update
            var logs = new AppLogModel
            {
                CreatedBy = $"{user.Id} - {user.FullName}",
                CreatedAt = time,
                Params = JsonConvert.SerializeObject(new
                {
                    oldData = JsonConvert.SerializeObject(new
                    {
                        dependent.EmployeeDependentId,
                        dependent.DependentName,
                        dependent.DependentGender,
                        dependent.DependentStatus,
                        dependent.EmployeeId,
                        dependent.CreatedBy,
                        dependent.CreatedAt,
                        dependent.UpdatedAt,
                        dependent.UpdatedBy
                    }),
                    newData = JsonConvert.SerializeObject(new
                    {
                        model.EmployeeDependentId,
                        model.DependentName,
                        model.DependentGender,
                        model.DependentStatus
                    })
                }),
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "EmployeeDependentController",
                    Action = "Edit",
                    Database = "EmployeeDependents",
                })
            };
            dependent.DependentName = model.DependentName;
            dependent.DependentGender = model.DependentGender;
            dependent.DependentStatus = model.DependentStatus;
            dependent.UpdatedBy = user.Id;
            dependent.UpdatedAt = time;
            context.EmployeeDependents.Update(dependent);
            await context.SaveChangesAsync();
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Dependent updated successfully";
            return RedirectToAction("Index", "Profile");
        }
        catch (Exception exception)
        {
            TempData["Errors"] = exception.Message;
            return RedirectToAction("Index", "Profile");
        }
    }

    // POST: EmployeeDependentController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id, IFormCollection collection)
    {
        try
        {
            var family = await context.EmployeeDependents.FindAsync(id);
            var current = DateTime.UtcNow;
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (family == null)
            {
                TempData["Errors"] = "Dependent not found";
                return RedirectToAction("Index", "Profile");
            }

            var logs = new AppLogModel
            {
                CreatedBy = $"{user.Id} - {user.FullName}",
                CreatedAt = current,
                Params = JsonConvert.SerializeObject(new
                {
                    oldData = JsonConvert.SerializeObject(new
                    {
                        family.EmployeeDependentId,
                        family.DependentName,
                        family.DependentGender,
                        family.DependentStatus
                    })
                }),
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "EmployeeDependentController",
                    Action = "Delete",
                    Database = "EmployeeDependents",
                })
            };
            await context.EmployeeDependents
                .Where(x => x.EmployeeDependentId == id).ExecuteDeleteAsync();
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Dependent deleted successfully";
            return RedirectToAction("Index", "Profile");
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return RedirectToAction("Index", "Profile");
        }
    }
}