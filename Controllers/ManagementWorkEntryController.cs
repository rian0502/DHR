using DHR.Helper;
using DHR.Models;
using DHR.Providers;
using DHR.ViewModels.ManagementWorkEntry;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DHR.Controllers
{
    public class ManagementWorkEntryController(
        UserManager<Users> userManager,
        AppDbContext context,
        MongoDbContext mongoDbContext,
        WorkEntryService workEntryService) : Controller
    {
        // GET: ManagementWorkEntryController
        public ActionResult Index()
        {
            ViewBag.RoleUser = User.IsInRole("AttendanceManager") ? "AttendanceManager" : "OtherUser";
            return View();
        }

        // GET: ManagementWorkEntryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManagementWorkEntryController/Create
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
            return View();
        }

        // POST: ManagementWorkEntryController/Create
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
                    return View(model);
                }
                var user = await userManager.GetUserAsync(User);
                var time = DateTime.UtcNow;
                if(user == null)
                {
                    return RedirectToAction("Logout", "Account");
                }
                await context.EmployeeWorkEntryRequests.AddAsync(new EmployeeWorkEntryRequest
                {
                    EmployeeWorkEntryCode = model.EmployeeWorkEntryCode,
                    WorkDate = model.WorkDate,
                    WorkStartTime = model.WorkStartTime,
                    WorkEndTime = model.WorkEndTime,
                    WorkReason = model.WorkReason,
                    PersonnelRemark = model.PersonnelRemark,
                    EmployeeId = model.EmployeeId,
                    CreatedBy = user.Id,
                    CreatedAt = time,
                    UpdatedBy = user.Id,
                    UpdatedAt = time,
                    IsDeleted = false
                });
                await context.SaveChangesAsync();

                await mongoDbContext.AppLogs.InsertOneAsync(new AppLogModel
                {
                    CreatedAt = time,
                    CreatedBy = $"{user.Id} - {user.FullName}",
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "ManagementWorkEntry",
                        Action = "Create",
                        Database = "EmployeeWorkEntryRequests",
                    }),
                    Params = JsonConvert.SerializeObject(model)
                });
                TempData["Success"] = "Data has been saved";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                TempData["Errors"] = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ManagementWorkEntryController/Edit/5
        public async Task<ActionResult> Edit(int id)
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
            var data = await context.EmployeeWorkEntryRequests.FindAsync(id);
            return View(new EditViewModel
            {
                EmployeeWorkEntryId = data.EmployeeWorkEntryId,
                EmployeeWorkEntryCode = data.EmployeeWorkEntryCode ?? "",
                WorkDate = data.WorkDate ?? DateOnly.FromDateTime(DateTime.Now),
                WorkStartTime = data.WorkStartTime ?? TimeOnly.MinValue,
                WorkEndTime = data.WorkEndTime ?? TimeOnly.FromDateTime(DateTime.Now.AddHours(8)),
                WorkReason = data.WorkReason ?? "",
                PersonnelRemark = data.PersonnelRemark ?? "",
                EmployeeId = data.EmployeeId
            });
        }

        // POST: ManagementWorkEntryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EditViewModel model)
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
                    return View(model);
                }
                var user = await userManager.GetUserAsync(User);
                var time = DateTime.UtcNow;

                if(user == null)
                {
                    return RedirectToAction("Logout", "Account");
                }
                var oldData = await context.EmployeeWorkEntryRequests.FindAsync(id);

                var logs = new AppLogModel
                {
                    CreatedAt = time,
                    CreatedBy = $"{user.Id} - {user.FullName}",
                    Source = JsonConvert.SerializeObject(new {
                        Controller = "ManagementWorkEntry",
                        Action = "Edit",
                        Database = "EmployeeWorkEntryRequests",
                    }),
                    Params = JsonConvert.SerializeObject(new
                    {
                        oldData = JsonConvert.SerializeObject(new {
                            oldData.EmployeeWorkEntryId,
                            oldData.EmployeeWorkEntryCode,
                            oldData.WorkDate,
                            oldData.WorkStartTime,
                            oldData.WorkEndTime,
                            oldData.WorkReason,
                            oldData.PersonnelRemark,
                            oldData.UpdatedAt,
                            oldData.UpdatedBy
                        }),
                        newData = JsonConvert.SerializeObject(model)
                    })
                };
                oldData.EmployeeWorkEntryCode = model.EmployeeWorkEntryCode;
                oldData.WorkDate = model.WorkDate;
                oldData.WorkStartTime = model.WorkStartTime;
                oldData.WorkEndTime = model.WorkEndTime;
                oldData.WorkReason = model.WorkReason;
                oldData.PersonnelRemark = model.PersonnelRemark;
                oldData.EmployeeId = model.EmployeeId;
                oldData.UpdatedAt = time;
                oldData.UpdatedBy = user.Id;
                context.EmployeeWorkEntryRequests.Update(oldData);
                await context.SaveChangesAsync();
                await mongoDbContext.AppLogs.InsertOneAsync(logs);
                TempData["Success"] = "Data has been updated";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["Errors"] = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ManagementWorkEntryController/Delete/5
        [Authorize(Roles = "AttendanceManager")]
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var data = await context.EmployeeWorkEntryRequests
            .Include(we => we.Employee)
            .ThenInclude(emp => emp.Users)
            .Where(we => we.EmployeeWorkEntryId == id)
            .Select(we => new EmployeeWorkEntryRequest
            {
                EmployeeWorkEntryId = we.EmployeeWorkEntryId,
                EmployeeWorkEntryCode = we.EmployeeWorkEntryCode,
                WorkDate = we.WorkDate,
                WorkStartTime = we.WorkStartTime,
                WorkEndTime = we.WorkEndTime,
                WorkReason = we.WorkReason,
                PersonnelRemark = we.PersonnelRemark,
                EmployeeId = we.EmployeeId,
                Employee = new EmployeeModel
                {
                    EmployeeId = we.Employee.EmployeeId,
                    Nip = we.Employee.Nip,
                    Users = new Users
                    {
                        FullName = we.Employee.Users.FullName
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

        // POST: ManagementWorkEntryController/Delete/5
        [Authorize(Roles = "AttendanceManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection model)
        {
            try
            {
                var reason = model["DeleteReason"].FirstOrDefault();
                if (string.IsNullOrEmpty(reason))
                {
                    ModelState.AddModelError("DeleteReason", "Delete Reason is Required");
                    var data = await context.EmployeeWorkEntryRequests
                        .Include(we => we.Employee)
                        .ThenInclude(emp => emp.Users)
                        .Where(we => we.EmployeeWorkEntryId == id)
                        .Select(we => new EmployeeWorkEntryRequest
                        {
                            EmployeeWorkEntryId = we.EmployeeWorkEntryId,
                            EmployeeWorkEntryCode = we.EmployeeWorkEntryCode,
                            WorkDate = we.WorkDate,
                            WorkStartTime = we.WorkStartTime,
                            WorkEndTime = we.WorkEndTime,
                            WorkReason = we.WorkReason,
                            PersonnelRemark = we.PersonnelRemark,
                            EmployeeId = we.EmployeeId,
                            Employee = new EmployeeModel
                            {
                                EmployeeId = we.Employee.EmployeeId,
                                Nip = we.Employee.Nip,
                                Users = new Users
                                {
                                    FullName = we.Employee.Users.FullName
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
                    return RedirectToAction("Logout", "Account");
                }

                var oldData = await context.EmployeeWorkEntryRequests.FindAsync(id);
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
                        oldData.EmployeeWorkEntryId,
                        oldData.EmployeeWorkEntryCode,
                        oldData.WorkDate,
                        oldData.WorkStartTime,
                        oldData.WorkEndTime,
                        oldData.WorkReason,
                        oldData.PersonnelRemark,
                        oldData.UpdatedAt,
                        oldData.UpdatedBy,
                        oldData.CreatedBy,
                        oldData.CreatedAt,
                        DeleteReason = reason
                    }),
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "ManagementLeaveRequest",
                        Action = "Delete",
                        Database = "EmployeeWorkEntryRequests"
                    })
                };
                //update data
                oldData.DeleteReason = reason;
                oldData.IsDeleted = true;
                oldData.UpdatedBy = user.Id;
                oldData.UpdatedAt = time;
                context.EmployeeWorkEntryRequests.Update(oldData);
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
        
        // GET: ManagementWorkEntryController/Import
        public ActionResult Import()
        {
            return View();
        }
        
        // POST: ManagementWorkEntryController/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Import(ImportViewModel model)
        {
            var data = await ReadWorkEntryFromExcelAsync(model.ExcelFile, User.Identity.Name);
            if (data.Count() != 0)
            {
                await workEntryService.InsertBatchWorkEntryAsync(data);
                TempData["Success"] = "Data has been saved";
                return RedirectToAction(nameof(Index));
            }
            return Ok(data);
            TempData["Errors"] = "Data is empty";
            return RedirectToAction(nameof(Index));
        }
        private async Task<List<object>> ReadWorkEntryFromExcelAsync(IFormFile excelFile, string users)
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
                            NIP = int.TryParse(reader.GetValue(1).ToString(), out var nip) ? nip : 0,
                            EmployeeWorkEntryCode = reader.GetValue(3).ToString(),
                            WorkDate = ParseDate(reader.GetValue(4).ToString()),
                            WorkStartTime = ParseTime(reader.GetValue(5).ToString()),
                            WorkEndTime = ParseTime(reader.GetValue(6).ToString()),
                            WorkReason = reader.GetValue(7).ToString(),
                            PersonnelRemark = reader.GetValue(8).ToString(),
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = users
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
        private TimeSpan? ParseTime(string timeString)
        {
            if (DateTime.TryParse(timeString, out var tempTime))
            {
                return tempTime.TimeOfDay;
            }
            return null;
        }
        [HttpPost]
        public async Task<IActionResult> GetWorkEntryRequests()
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
                "EmployeeWorkEntryId",
                "EmployeeWorkEntryCode",
                "WorkDate",
                "Employee.Nip",
                "Employee.Users.FullName",
                "WorkStartTime",
                "WorkEndTime"
            };

            var query = context.EmployeeWorkEntryRequests
                .Where(l => l.IsDeleted == false)
                .Include(e => e.Employee)
                .ThenInclude(e => e.Users)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(e => e.Employee.Users != null && searchValue != null &&
                                         e.EmployeeWorkEntryCode != null &&
                                         (e.Employee.Users.FullName.Contains(searchValue) ||
                                          e.WorkDate.ToString().Contains(searchValue) ||
                                          e.Employee.Nip.ToString().Contains(searchValue) ||
                                          e.WorkStartTime.ToString().Contains(searchValue) ||
                                          e.WorkEndTime.ToString().Contains(searchValue)));
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
                else if (sortColumn == "EmployeeWorkEntryCode")
                {
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(e => e.EmployeeWorkEntryCode)
                        : query.OrderByDescending(e => e.EmployeeWorkEntryCode);
                }
                else if (sortColumn == "WorkDate")
                {
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(e => e.WorkDate)
                        : query.OrderByDescending(e => e.WorkDate);
                }
                else if (sortColumn == "WorkStartTime")
                {
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(e => e.WorkStartTime)
                        : query.OrderByDescending(e => e.WorkStartTime);
                }
                else if (sortColumn == "WorkEndTime")
                {
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(e => e.WorkEndTime)
                        : query.OrderByDescending(e => e.WorkEndTime);
                }
                
            }

            // Pagination
            var pagedData = await
                query.Skip(Convert.ToInt32(start))
                    .Take(Convert.ToInt32(length)).Select(emc => new
                    {
                        emc.EmployeeWorkEntryCode,
                        emc.EmployeeWorkEntryId,
                        EmployeeNip = emc.Employee.Nip,
                        EmployeeName = emc.Employee.Users.FullName,
                        emc.WorkDate,
                        emc.WorkStartTime,
                        emc.WorkEndTime
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
    }
}