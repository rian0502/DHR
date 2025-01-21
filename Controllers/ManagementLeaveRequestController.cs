using DHR.Helper;
using DHR.Models;
using DHR.Providers;
using DHR.ViewModels.ManagementLeaveRequest;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DHR.Controllers
{
    [Authorize(Roles = "Admin, AttendanceAdministrator, AttendanceManager")]
    public class ManagementLeaveRequestController(
        AppDbContext context,
        MongoDbContext mongoDbContext,
        UserManager<Users> userManager,
        LeaveRequestService leaveRequestService) : Controller
    {
        // GET: ManagementLeaveRequest
        public ActionResult Index()
        {
            ViewBag.RoleUser = User.IsInRole("AttendanceManager") ? "AttendanceManager" : "OtherUser";
            return View();
        }

        // GET: ManagementLeaveRequest/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.LeaveTypes = new List<string>
            {
                "Cuti",
                "Cuti Bersama",
                "Cuti Melahirkan",
                "Cuti Menikah"
            };

            ViewBag.Employee = await context.Employee
                .Include(e => e.Users)
                .Where(model => model.Users != null && model.Users.UserName != "admin")
                .Select(model => new
                {
                    model.EmployeeId,
                    model.Nip,
                    model.Users.FullName,
                    model.Users.Id
                }).OrderBy(employee => employee.Nip)
                .ToListAsync();
            return View();
        }

        // POST: ManagementLeaveRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.LeaveTypes = new List<string>
                    {
                        "Cuti",
                        "Cuti Bersama",
                        "Cuti Melahirkan",
                        "Cuti Menikah"
                    };
                    ViewBag.Employee = await context.Employee
                        .Include(e => e.Users)
                        .Where(employeeModel => employeeModel.Users != null && employeeModel.Users.UserName != "admin")
                        .Select(employeeModel => new
                        {
                            employeeModel.EmployeeId,
                            employeeModel.Nip,
                            employeeModel.Users.FullName,
                            employeeModel.Users.Id
                        })
                        .ToListAsync();
                    return View(model);
                }

                var time = DateTime.UtcNow;
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                var leaveRequest = new EmployeeLeaveRequestModel
                {
                    EmployeeLeaveRequestCode = model.LeaveCode,
                    LeaveDate = model.LeaveDate,
                    LeaveDays = model.LeaveDays,
                    LeaveReason = model.LeaveReason,
                    LeaveType = model.LeaveType,
                    EmployeeId = model.EmployeeId,
                    CreatedBy = user.Id,
                    CreatedAt = time,
                    UpdatedBy = user.Id,
                    UpdatedAt = time
                };
                await context.EmployeeLeaveRequest.AddAsync(leaveRequest);
                await context.SaveChangesAsync();

                var logs = new AppLogModel
                {
                    CreatedBy = $"{user.Id} - {user.FullName}",
                    CreatedAt = time,
                    Params = JsonConvert.SerializeObject(new
                    {
                        model.EmployeeId,
                        model.LeaveDate,
                        model.LeaveDays,
                        model.LeaveType,
                        model.LeaveReason,
                        model.LeaveCode
                    }),
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "ManagementLeaveRequest",
                        Action = "Create",
                        Database = "EmployeeLeaveRequest"
                    })
                };
                await mongoDbContext.AppLogs.InsertOneAsync(logs);
                TempData["Success"] = "Leave request created successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                TempData["Errors"] = exception.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ManagementLeaveRequest/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await context.EmployeeLeaveRequest.FindAsync(id);

            ViewBag.LeaveTypes = new List<string>
            {
                "Cuti",
                "Cuti Bersama",
                "Cuti Melahirkan",
                "Cuti Menikah"
            };
            ViewBag.Employee = await context.Employee
                .Include(e => e.Users)
                .Where(employeeModel => employeeModel.Users != null && employeeModel.Users.UserName != "admin")
                .Select(employeeModel => new
                {
                    employeeModel.EmployeeId,
                    employeeModel.Nip,
                    employeeModel.Users.FullName,
                    employeeModel.Users.Id
                })
                .ToListAsync();


            return View(new EditViewModel
            {
                EmployeeLeaveRequestId = model.EmployeeLeaveRequestId,
                LeaveCode = model.EmployeeLeaveRequestCode ?? "",
                EmployeeId = model.EmployeeId,
                LeaveDate = model.LeaveDate,
                LeaveDays = model.LeaveDays,
                LeaveType = model.LeaveType ?? "",
                LeaveReason = model.LeaveReason ?? ""
            });
        }

        // POST: ManagementLeaveRequest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.GetUserAsync(User);
                    var time = DateTime.UtcNow;
                    if (user == null)
                    {
                        return RedirectToAction("Logout", "Account");
                    }

                    var oldData = await context.EmployeeLeaveRequest.FindAsync(id);
                    if (oldData == null)
                    {
                        TempData["Errors"] = "Data not found";
                        return RedirectToAction(nameof(Index));
                    }

                    var logs = new AppLogModel
                    {
                        CreatedAt = time,
                        CreatedBy = $"{user.Id} - {user.FullName}",
                        Params = JsonConvert.SerializeObject(new
                        {
                            OldData = JsonConvert.SerializeObject(new
                            {
                                oldData.EmployeeLeaveRequestCode,
                                oldData.LeaveType,
                                oldData.LeaveDate,
                                oldData.LeaveDays,
                                oldData.LeaveReason,
                                oldData.EmployeeId,
                                oldData.CreatedAt,
                                oldData.CreatedBy,
                                oldData.UpdatedAt,
                                oldData.UpdatedBy
                            }),
                            NewData = model
                        }),
                        Source = JsonConvert.SerializeObject(new
                        {
                            Controller = "ManagementLeaveRequest",
                            Action = "Edit",
                            Database = "EmployeeLeaveRequest"
                        })
                    };
                    //update data
                    oldData.EmployeeLeaveRequestCode = model.LeaveCode;
                    oldData.EmployeeId = model.EmployeeId;
                    oldData.LeaveDate = model.LeaveDate;
                    oldData.LeaveDays = model.LeaveDays;
                    oldData.LeaveType = model.LeaveType;
                    oldData.LeaveReason = model.LeaveReason;
                    oldData.UpdatedBy = user.Id;
                    oldData.UpdatedAt = time;
                    context.EmployeeLeaveRequest.Update(oldData);
                    await context.SaveChangesAsync();
                    await mongoDbContext.AppLogs.InsertOneAsync(logs);
                    TempData["Success"] = "Data updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                var leaveRequest = await context.EmployeeLeaveRequest.FindAsync(id);

                ViewBag.LeaveTypes = new List<string>
                {
                    "Cuti",
                    "Cuti Bersama",
                    "Cuti Melahirkan",
                    "Cuti Menikah"
                };
                ViewBag.Employee = await context.Employee
                    .Include(e => e.Users)
                    .Where(employeeModel => employeeModel.Users != null && employeeModel.Users.UserName != "admin")
                    .Select(employeeModel => new
                    {
                        employeeModel.EmployeeId,
                        employeeModel.Nip,
                        employeeModel.Users.FullName,
                        employeeModel.Users.Id
                    })
                    .ToListAsync();


                return View(new EditViewModel
                {
                    EmployeeLeaveRequestId = leaveRequest.EmployeeLeaveRequestId,
                    LeaveCode = leaveRequest.EmployeeLeaveRequestCode ?? "",
                    EmployeeId = leaveRequest.EmployeeId,
                    LeaveDate = leaveRequest.LeaveDate,
                    LeaveDays = leaveRequest.LeaveDays,
                    LeaveType = leaveRequest.LeaveType ?? "",
                    LeaveReason = leaveRequest.LeaveReason ?? ""
                });
            }
            catch (Exception e)
            {
                TempData["Errors"] = e.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: ManagementLeaveRequestController/Import
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(ImportLeaveRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Logout", "Account");
                }
                var leaves = await ReadLeaveRequestFromExcelAsync(model.ExcelFile, user.Id);
                if (leaves.Count != 0)
                {

                    await leaveRequestService.InsertBatchLeaveRequestsAsync(leaves);
                    TempData["Success"] = "Data successfully imported";
                }
                else
                {
                    TempData["Errors"] = "No data found in the file";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Errors"] = $"Error during import: {ex.Message}";
                return View(model);
            }
        }


        // GET: ManagementLeaveRequest/Delete/5
        [Authorize(Roles = "AttendanceManager")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.EmployeeLeaveRequest.Include(emp => emp.Employee)
                .ThenInclude(usr => usr.Users)
                
                .Where(emp => emp.EmployeeLeaveRequestId == id)
                .Select(emp => new EmployeeLeaveRequestModel
                {
                    EmployeeLeaveRequestId = emp.EmployeeLeaveRequestId,
                    EmployeeId = emp.EmployeeId,
                    LeaveDate = emp.LeaveDate,
                    LeaveDays = emp.LeaveDays,
                    LeaveType = emp.LeaveType,
                    LeaveReason = emp.LeaveReason,
                    Employee = new EmployeeModel
                    {
                        Nip = emp.Employee.Nip,
                        Users = new Users
                        {
                            FullName = emp.Employee.Users.FullName
                        }
                    }
                }).FirstOrDefaultAsync();
            return View(model);
        }
        
        // POST: ManagementLeaveRequest/Delete/5
        [Authorize(Roles = "AttendanceManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection model)
        {
            try
            {
                var reason = model["DeleteReason"].FirstOrDefault();
                if (string.IsNullOrEmpty(reason))
                {
                    ModelState.AddModelError("DeleteReason", "Delete Reason is Required");
                    var leaveRequest = await context.EmployeeLeaveRequest.Include(emp => emp.Employee)
                        .ThenInclude(usr => usr.Users)
                
                        .Where(emp => emp.EmployeeLeaveRequestId == id)
                        .Select(emp => new EmployeeLeaveRequestModel
                        {
                            EmployeeLeaveRequestId = emp.EmployeeLeaveRequestId,
                            EmployeeId = emp.EmployeeId,
                            LeaveDate = emp.LeaveDate,
                            LeaveDays = emp.LeaveDays,
                            LeaveType = emp.LeaveType,
                            LeaveReason = emp.LeaveReason,
                            Employee = new EmployeeModel
                            {
                                Nip = emp.Employee.Nip,
                                Users = new Users
                                {
                                    FullName = emp.Employee.Users.FullName
                                }
                            }
                        }).FirstOrDefaultAsync();
                    return View(leaveRequest);
                }

                var user = await userManager.GetUserAsync(User);
                var time = DateTime.UtcNow;
                if (user == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                var oldData = await context.EmployeeLeaveRequest.FindAsync(id);
                if (oldData == null)
                {
                    TempData["Errors"] = "Data not found";
                    return RedirectToAction(nameof(Index));
                }

                var logs = new AppLogModel
                {
                    CreatedAt = time,
                    CreatedBy = $"{user.Id} - {user.FullName}",
                    Params = JsonConvert.SerializeObject(new
                    {
                        oldData.LeaveType,
                        oldData.LeaveDate,
                        oldData.LeaveDays,
                        oldData.LeaveReason,
                        oldData.EmployeeLeaveRequestCode,
                        oldData.EmployeeLeaveRequestId,
                        DeleteReason = reason
                    }),
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "ManagementLeaveRequest",
                        Action = "Delete",
                        Database = "EmployeeLeaveRequest"
                    })
                };
                //update data
                oldData.IsDeleted = true;
                oldData.UpdatedBy = user.Id;
                oldData.UpdatedAt = time;
                context.EmployeeLeaveRequest.Update(oldData);
                await context.SaveChangesAsync();
                await mongoDbContext.AppLogs.InsertOneAsync(logs);
                TempData["Success"] = "Data deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["Errors"] = e.Message;
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetLeaveRequests()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnIndex = Convert.ToInt32(HttpContext.Request.Form["order[0][column]"].FirstOrDefault());
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            sortColumnDirection = (sortColumnDirection == "desc") ? "desc" : "asc";

            string[] columnNames =
            {
                "EmployeeLeaveRequestCode",
                "EmployeeLeaveRequestId",
                "Employee.Nip",
                "Employee.Users.FullName",
                "LeaveDate",
                "LeaveDays",
                "LeaveType"
            };

            var query = context.EmployeeLeaveRequest
                .Where(l => l.IsDeleted == false)
                .Include(e => e.Employee)
                .ThenInclude(e => e.Users)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(e => e.Employee.Users != null && searchValue != null &&
                                         e.LeaveType != null &&
                                         (e.Employee.Users.FullName.Contains(searchValue) ||
                                          e.EmployeeLeaveRequestCode.Contains(searchValue) ||
                                          e.Employee.Nip.ToString().Contains(searchValue) ||
                                          e.LeaveDate.ToString().Contains(searchValue) ||
                                          e.LeaveDays.ToString().Contains(searchValue) ||
                                          e.LeaveType.Contains(searchValue)));
            }

            var totalRecords = await query.CountAsync();

            if (sortColumnIndex >= 0 & sortColumnIndex < columnNames.Length)
            {
                string sortColumn = columnNames[sortColumnIndex];
                if (sortColumn == "Employee.Nip")
                {
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(e => e.Employee.Nip)
                        : query.OrderByDescending(e => e.Employee.Nip);
                }
                else if (sortColumn == "Employee.Users.FullName")
                {
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(e => e.Employee.Users.FullName)
                        : query.OrderByDescending(e => e.Employee.Users.FullName);
                }
                else if (sortColumn == "LeaveDate")
                {
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(e => e.LeaveDate)
                        : query.OrderByDescending(e => e.LeaveDate);
                }
                else if (sortColumn == "LeaveDays")
                {
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(e => e.LeaveDays)
                        : query.OrderByDescending(e => e.LeaveDays);
                }
                else if (sortColumn == "LeaveType")
                {
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(e => e.LeaveType)
                        : query.OrderByDescending(e => e.LeaveType);
                }
                else if (sortColumn == "EmployeeLeaveRequestCode")
                {
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(e => e.EmployeeLeaveRequestCode)
                        : query.OrderByDescending(e => e.EmployeeLeaveRequestCode);
                }
            }

            // Pagination
            var pagedData = await
                query.Skip(Convert.ToInt32(start))
                    .Take(Convert.ToInt32(length)).Select(emc => new
                    {
                        emc.EmployeeLeaveRequestCode,
                        LeaveRequestId = emc.EmployeeLeaveRequestId,
                        EmployeeNip = emc.Employee.Nip,
                        EmployeeName = emc.Employee.Users.FullName,
                        emc.LeaveDate,
                        emc.LeaveDays,
                        emc.LeaveType
                    }).ToListAsync();
            var response = new
            {
                draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = pagedData
            };

            return Json(response);
        }

        private async Task<List<object>> ReadLeaveRequestFromExcelAsync(IFormFile excelFile, string user)
        {
            var claims = new List<object>();

            await using var stream = excelFile.OpenReadStream();
            using var reader = ExcelReaderFactory.CreateReader(stream);
            bool isFirstSheet = true;
            do
            {
                if (isFirstSheet)
                {
                    reader.Read();

                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0) || reader.IsDBNull(1)) continue;

                        var claim = new
                        {
                            Nip = int.TryParse(reader.GetValue(1)?.ToString(), out var nip) ? nip : 0,
                            Code = reader.GetValue(3).ToString(),
                            LeaveDate = ParseDate(reader.GetValue(4).ToString()),
                            LeaveDays = double.TryParse(reader.GetValue(5).ToString().Replace(",", "."), out var days)
                                ? days
                                : 0,
                            LeaveType = reader.GetValue(6).ToString(),
                            LeaveReason = reader.GetValue(7).ToString(),
                            CreatedBy = user,
                            CreatedAt = DateTime.UtcNow
                        };

                        claims.Add(claim);
                    }

                    isFirstSheet = false;
                }
            } while (reader.NextResult());

            return claims;
        }

        private DateOnly? ParseDate(string dateString)
        {
            return DateTime.TryParse(dateString, out var tempDate)
                ? DateOnly.FromDateTime(tempDate)
                : null;
        }
    }
}