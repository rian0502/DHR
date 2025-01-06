using DHR.Helper;
using DHR.Models;
using DHR.Providers;
using DHR.ViewModels.ManagementLeaveRequest;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DHR.Controllers
{
    [Authorize(Roles = "Admin, AttendanceAdministrator")]
    public class ManagementLeaveRequestController(
        AppDbContext context,
        MongoDbContext mongoDbContext,
        UserManager<Users> userManager,
        LeaveRequestService leaveRequestService) : Controller
    {
        // GET: ManagementLeaveRequest
        public ActionResult Index()
        {
            return View();
        }

        // GET: ManagementLeaveRequest/Details/5
        public ActionResult Details(int id)
        {
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
                    ViewBag.Users = await context.Employee
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
                        model.LeaveReason
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
        public ActionResult Edit(int id)
        {

            return View();
        }

        // POST: ManagementLeaveRequest/Edit/5
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

        // GET: ManagementLeaveRequest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManagementLeaveRequest/Delete/5
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
                var claims = await ReadLeaveRequestFromExcelAsync(model.ExcelFile);
                if (claims.Count != 0)
                {
                    await leaveRequestService.InsertBatchLeaveRequestsAsync(claims);
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
                "EmployeeLeaveRequestId",
                "Employee.Nip",
                "Employee.Users.FullName",
                "LeaveDate",
                "LeaveDays",
                "LeaveType"
            };

            var query = context.EmployeeLeaveRequest
                .Include(e => e.Employee)
                .ThenInclude(e => e.Users)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(e => e.Employee.Users != null && searchValue != null &&
                                         e.LeaveType != null &&
                                         (e.Employee.Users.FullName.Contains(searchValue) ||
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
            }

            // Pagination
            var pagedData = await
                query.Skip(Convert.ToInt32(start))
                    .Take(Convert.ToInt32(length)).Select(emc => new
                    {
                        LeaveRequestId = emc.EmployeeLeaveRequestId,
                        EmployeNip = emc.Employee.Nip,
                        EmployeeName = emc.Employee.Users.FullName,
                        LeaveDate = emc.LeaveDate,
                        LeaveDays = emc.LeaveDays,
                        LeaveType = emc.LeaveType
                    }).ToListAsync();
            var response = new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = pagedData
            };

            return Json(response);
        }

        private async Task<List<object>> ReadLeaveRequestFromExcelAsync(IFormFile excelFile)
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
                            LeaveDate = ParseDate(reader.GetValue(3).ToString()),
                            LeaveDays = double.TryParse(reader.GetValue(4)?.ToString().Replace(",", "."), out var days)
                                ? days
                                : 0,
                            LeaveType = reader.GetValue(5).ToString(),
                            LeaveReason = reader.GetValue(6).ToString(),
                            CreatedBy = "admin",
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