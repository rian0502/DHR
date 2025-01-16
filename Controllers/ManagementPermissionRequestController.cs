using DHR.Helper;
using DHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DHR.ViewModels.ManagementPermissionRequest;
using Newtonsoft.Json;

namespace DHR.Controllers;

[Authorize(Roles = "Admin, AttendanceAdministrator, AttendanceManager")]
public class ManagementPermissionRequestController(
    UserManager<Users> userManager,
    AppDbContext context,
    MongoDbContext mongoContext) : Controller
{
    // GET: ManagementPermissionRequestController
    public async Task<ActionResult> Index()
    {
        var permissions = await context.EmployeePermissionRequest
            .Include(emp => emp.Employee)
            .ThenInclude(usr => usr.Users)
            .Where(epr => !epr.IsDeleted).Select(epr => new EmployeePermissionRequest
            {
                EmployeePermissionRequestId = epr.EmployeePermissionRequestId,
                EmployeePermissionRequestCode = epr.EmployeePermissionRequestCode,
                PermissionType = epr.PermissionType,
                PermissionDays = epr.PermissionDays,
                PermissionDate = epr.PermissionDate,
                EmployeeId = epr.EmployeeId,
                Employee = new EmployeeModel
                {
                    EmployeeId = epr.Employee.EmployeeId,
                    Nip = epr.Employee.Nip,
                    Users = new Users
                    {
                        FullName = epr.Employee.Users.FullName
                    }
                }
            })
            .ToListAsync();
        return View(permissions);
    }

    // GET: ManagementPermissionRequestController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: ManagementPermissionRequestController/Create
    public async Task<ActionResult> Create()
    {
        ViewBag.Employee = await context.Employee.Include(e => e.Users)
            .Where(model => model.Users != null && model.Users.UserName != "admin" && !model.IsDeleted)
            .Select(model => new
            {
                model.EmployeeId,
                model.Nip,
                model.Users.FullName,
                model.Users.Id
            }).OrderBy(employee => employee.Nip)
            .ToListAsync();
        ViewBag.PermissionTypes = new List<string>
        {
            "Izin Terlambat",
            "Izin Tidak Masuk kerja",
            "Izin Pulang Cepat",
            "Izin Keluar Kantor"
        };
        return View();
    }

    // POST: ManagementPermissionRequestController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Employee = await context.Employee.Include(e => e.Users)
                    .Where(model => model.Users != null && model.Users.UserName != "admin" && !model.IsDeleted)
                    .Select(model => new
                    {
                        model.EmployeeId,
                        model.Nip,
                        model.Users.FullName,
                        model.Users.Id
                    }).OrderBy(employee => employee.Nip)
                    .ToListAsync();
                ViewBag.PermissionTypes = new List<string>
                {
                    "Izin Terlambat",
                    "Izin Tidak Masuk kerja",
                    "Izin Pulang Cepat",
                    "Izin Keluar Kantor"
                };
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            var time = DateTime.UtcNow;
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            await context.EmployeePermissionRequest.AddAsync(new EmployeePermissionRequest
            {
                EmployeePermissionRequestCode = model.EmployeePermissionRequestCode,
                EmployeeId = model.EmployeeId,
                PermissionType = model.PermissionType,
                PermissionDays = model.PermissionDays,
                PermissionDate = model.PermissionDate,
                PermissionReason = model.PermissionReason,
                CreatedBy = user.Id,
                CreatedAt = time,
                UpdatedBy = user.Id,
                UpdatedAt = time
            });
            await context.SaveChangesAsync();
            var logs = new AppLogModel
            {
                CreatedAt = time,
                CreatedBy = $"{user.Id} - {user.FullName}",
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "ManagementPermissionRequest",
                    Action = "Create",
                    Database = "EmployeePermissionRequest",
                }),
                Params = JsonConvert.SerializeObject(model),
            };
            await mongoContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Data has been saved";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: ManagementPermissionRequestController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        var data = await context.EmployeePermissionRequest
            .Include(emp => emp.Employee)
            .ThenInclude(usr => usr.Users)
            .Where(epr => epr.EmployeePermissionRequestId == id)
            .Select(epr => new EmployeePermissionRequest
            {
                EmployeePermissionRequestId = epr.EmployeePermissionRequestId,
                EmployeePermissionRequestCode = epr.EmployeePermissionRequestCode,
                PermissionType = epr.PermissionType,
                PermissionDays = epr.PermissionDays,
                PermissionDate = epr.PermissionDate,
                PermissionReason = epr.PermissionReason,
                EmployeeId = epr.EmployeeId,
                Employee = new EmployeeModel
                {
                    EmployeeId = epr.Employee.EmployeeId,
                    Nip = epr.Employee.Nip,
                    Users = new Users
                    {
                        FullName = epr.Employee.Users.FullName
                    }
                }
            })
            .FirstOrDefaultAsync();
        if (data == null)
        {
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Employee = await context.Employee.Include(e => e.Users)
            .Where(model => model.Users != null && model.Users.UserName != "admin" && !model.IsDeleted)
            .Select(model => new
            {
                model.EmployeeId,
                model.Nip,
                model.Users.FullName,
                model.Users.Id
            }).OrderBy(employee => employee.Nip)
            .ToListAsync();
        ViewBag.PermissionTypes = new List<string>
        {
            "Izin Terlambat",
            "Izin Tidak Masuk kerja",
            "Izin Pulang Cepat",
            "Izin Keluar Kantor"
        };
        return View(new EditViewModel
        {
            EmployeePermissionRequestId = data.EmployeePermissionRequestId,
            EmployeePermissionRequestCode = data.EmployeePermissionRequestCode,
            EmployeeId = data.EmployeeId,
            PermissionType = data.PermissionType,
            PermissionDays = data.PermissionDays,
            PermissionDate = data.PermissionDate,
            PermissionReason = data.PermissionReason
        });
    }

    // POST: ManagementPermissionRequestController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
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

    // GET: ManagementPermissionRequestController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: ManagementPermissionRequestController/Delete/5
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