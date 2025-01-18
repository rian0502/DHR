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
        ViewBag.PermissionRemarks = new List<string>
        {
            "Potong Cuti",
            "Potong Gaji",
            "Dispensasi"
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
                ViewBag.PermissionRemarks = new List<string>
                {
                    "Potong Cuti",
                    "Potong Gaji",
                    "Dispensasi"
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
                PersonnelRemarks = model.PersonnelRemarks,
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
                PersonnelRemarks = epr.PersonnelRemarks,
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
        ViewBag.PermissionRemarks = new List<string>
        {
            "Potong Cuti",
            "Potong Gaji",
            "Dispensasi"
        };
        return View(new EditViewModel
        {
            EmployeePermissionRequestId = data.EmployeePermissionRequestId,
            EmployeePermissionRequestCode = data.EmployeePermissionRequestCode ?? "",
            EmployeeId = data.EmployeeId,
            PersonnelRemarks = data.PersonnelRemarks ?? "",
            PermissionType = data.PermissionType ?? "",
            PermissionDays = data.PermissionDays,
            PermissionDate = data.PermissionDate,
            PermissionReason = data.PermissionReason ?? ""
        });
    }

    // POST: ManagementPermissionRequestController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, EditViewModel model)
    {
        try
        {
            var user = await userManager.GetUserAsync(User);
            var time = DateTime.UtcNow;
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

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
                ViewBag.PermissionRemarks = new List<string>
                {
                    "Potong Cuti",
                    "Potong Gaji",
                    "Dispensasi"
                };
                return View(model);
            }

            var oldData = await context.EmployeePermissionRequest.FindAsync(id);
            var logs = new AppLogModel
            {
                CreatedBy = $"{user.Id} - {user.FullName}",
                CreatedAt = time,
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "ManagementPermissionRequest",
                    Action = "Edit",
                    Database = "EmployeePermissionRequest"
                }),
                Params = JsonConvert.SerializeObject(new
                {
                    OldData = JsonConvert.SerializeObject(new
                    {
                        oldData.EmployeePermissionRequestId,
                        oldData.EmployeePermissionRequestCode,
                        oldData.PermissionType,
                        oldData.PermissionDays,
                        oldData.PermissionDate,
                        oldData.PersonnelRemarks,
                        oldData.PermissionReason,
                        oldData.EmployeeId,
                        oldData.CreatedAt,
                        oldData.CreatedBy,
                        oldData.UpdatedAt,
                        oldData.UpdatedBy
                    }),
                    NewData = model
                })
            };

            oldData.EmployeePermissionRequestCode = model.EmployeePermissionRequestCode;
            oldData.EmployeeId = model.EmployeeId;
            oldData.PermissionType = model.PermissionType;
            oldData.PermissionDays = model.PermissionDays;
            oldData.PermissionDate = model.PermissionDate;
            oldData.PermissionReason = model.PermissionReason;
            oldData.PersonnelRemarks = model.PersonnelRemarks;
            oldData.UpdatedAt = time;
            oldData.UpdatedBy = user.Id;
            context.EmployeePermissionRequest.Update(oldData);
            await context.SaveChangesAsync();
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

    //[Authorize(Roles = "AttendanceManager")]
    // GET: ManagementPermissionRequestController/Delete/5
    public async Task<ActionResult> Delete(int id)
    {
        var data = await context.EmployeePermissionRequest
            .Include(epr => epr.Employee)
            .ThenInclude(emp => emp.Users)
            .Where(epr => epr.EmployeePermissionRequestId == id)
            .Select(epr => new EmployeePermissionRequest
            {
                EmployeePermissionRequestId = epr.EmployeePermissionRequestId,
                EmployeePermissionRequestCode = epr.EmployeePermissionRequestCode,
                PermissionType = epr.PermissionType,
                PermissionDays = epr.PermissionDays,
                PermissionDate = epr.PermissionDate,
                PersonnelRemarks = epr.PersonnelRemarks,
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

        return View(data);
    }

    // POST: ManagementPermissionRequestController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    //[Authorize(Roles = "AttendanceManager")]
    public async Task<ActionResult> Delete(int id, IFormCollection model)
    {
        try
        {
            var reason = model["DeleteReason"].FirstOrDefault();
            if (string.IsNullOrEmpty(reason))
            {
                ModelState.AddModelError("DeleteReason", "Delete reason is required");
                var data = await context.EmployeePermissionRequest
                    .Include(epr => epr.Employee)
                    .ThenInclude(emp => emp.Users)
                    .Where(epr => epr.EmployeePermissionRequestId == id)
                    .Select(epr => new EmployeePermissionRequest
                    {
                        EmployeePermissionRequestId = epr.EmployeePermissionRequestId,
                        EmployeePermissionRequestCode = epr.EmployeePermissionRequestCode,
                        PermissionType = epr.PermissionType,
                        PermissionDays = epr.PermissionDays,
                        PermissionDate = epr.PermissionDate,
                        PersonnelRemarks = epr.PersonnelRemarks,
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
                return View(data);
            }

            var user = await userManager.GetUserAsync(User);
            var time = DateTime.UtcNow;
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var delete = await context.EmployeePermissionRequest.FindAsync(id);
            var logs = new AppLogModel
            {
                CreatedBy = $"{user.Id} - {user.FullName}",
                CreatedAt = time,
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "ManagementPermissionRequest",
                    Action = "Delete",
                    Database = "EmployeePermissionRequest"
                }),
                Params = JsonConvert.SerializeObject(new
                {
                    delete.EmployeePermissionRequestId,
                    delete.EmployeePermissionRequestCode,
                    delete.PermissionType,
                    delete.PermissionDays,
                    delete.PermissionDate,
                    delete.PersonnelRemarks,
                    delete.PermissionReason,
                    delete.EmployeeId,
                    delete.CreatedAt,
                    delete.CreatedBy,
                    delete.UpdatedAt,
                    delete.UpdatedBy,
                    DeleteReason = reason
                })
            };
            delete.IsDeleted = true;
            delete.UpdatedAt = time;
            delete.UpdatedBy = user.Id;
            delete.DeleteReason = reason;
            context.EmployeePermissionRequest.Update(delete);
            await context.SaveChangesAsync();
            await mongoContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Data has been deleted";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}