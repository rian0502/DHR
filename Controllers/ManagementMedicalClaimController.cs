using DHR.Helper;
using DHR.Models;
using DHR.Providers;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DHR.ViewModels.ManagementMedicalClaim;
using DHR.ViewModels.ManagementImportViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DHR.Controllers
{
    [Authorize(Roles = "Admin, ClaimAdministrator, ClaimManager")]
    public class ManagementMedicalClaimController(
        AppDbContext context,
        MongoDbContext mongoDbContext,
        UserManager<Users> userManager,
        MedicalClaimService medicalClaimService) : Controller
    {
        // GET: ManagementClaimController
        public ActionResult Index()
        {
            ViewBag.RoleUser = User.IsInRole("ClaimManager") ? "ClaimManager" : "OtherUser";
            return View();
        }

        // GET: ManagementClaimController/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Categories = new List<string> { "RAWAT_JALAN", "RAWAT_INAP", "MELAHIRKAN", "KACAMATA" };

            var periods = await context.Periods
                .Select(period => new
                {
                    period.PeriodId,
                    period.IsActive,
                    period.StartPeriodDate,
                    period.EndPeriodDate
                })
                .OrderBy(period => period.StartPeriodDate)
                .ToListAsync();

            var selectedPeriodId = periods.FirstOrDefault(p => p.IsActive)?.PeriodId;

            ViewBag.Periods = new SelectList(
                periods.Select(p => new
                {
                    p.PeriodId,
                    DisplayText = $"{p.StartPeriodDate:dd MMM yyyy} - {p.EndPeriodDate:dd MMM yyyy}"
                }),
                "PeriodId",
                "DisplayText",
                selectedPeriodId
            );


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

        // POST: ManagementClaimController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        return RedirectToAction("Logout", "Account");
                    }

                    var time = DateTime.UtcNow;
                    await context.EmployeeMedicalClaims.AddAsync(new EmployeeMedicalClaim
                    {
                        EmployeeId = model.EmployeeId,
                        ClaimDate = model.ClaimDate,
                        ClaimStatus = model.ClaimStatus,
                        ClaimCategory = model.ClaimCategory,
                        ClaimDescription = model.ClaimDescription,
                        EmployeeMedicalClaimCode = model.EmployeeMedicalClaimCode,
                        Diagnosis = model.Diagnosis,
                        PaymentPeriod = model.PaymentPeriod,
                        PeriodId = model.PeriodId,
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
                            model.PeriodId,
                            model.EmployeeId,
                            model.ClaimDate,
                            model.PaymentPeriod,
                            model.Diagnosis,
                            model.ClaimCategory,
                            model.ClaimDescription,
                            model.ClaimStatus
                        }),
                        Source = JsonConvert.SerializeObject(new
                        {
                            Controller = "ManagementMedicalClaim",
                            Action = "Create",
                            Database = "EmployeeMedicalClaims"
                        })
                    };
                    await mongoDbContext.AppLogs.InsertOneAsync(logs);
                    TempData["Success"] = "Data has been saved successfully";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Categories = new List<string> { "RAWAT_JALAN", "RAWAT_INAP", "MELAHIRKAN", "KACAMATA" };

                var periods = await context.Periods
                    .Select(period => new
                    {
                        period.PeriodId,
                        period.IsActive,
                        period.StartPeriodDate,
                        period.EndPeriodDate
                    })
                    .OrderBy(period => period.StartPeriodDate)
                    .ToListAsync();

                var selectedPeriodId = periods.FirstOrDefault(p => p.IsActive)?.PeriodId;

                ViewBag.Periods = new SelectList(
                    periods.Select(p => new
                    {
                        p.PeriodId,
                        DisplayText = $"{p.StartPeriodDate:dd MMM yyyy} - {p.EndPeriodDate:dd MMM yyyy}"
                    }),
                    "PeriodId",
                    "DisplayText",
                    selectedPeriodId
                );


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
                return View(model);
            }
            catch (Exception exception)
            {
                TempData["Errors"] = exception.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ManagementClaimController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var medicalClaim = await context.EmployeeMedicalClaims.FindAsync(id);
            if (medicalClaim == null)
            {
                TempData["Errors"] = "Data not found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new List<string> { "RAWAT_JALAN", "RAWAT_INAP", "MELAHIRKAN", "KACAMATA" };
            var periods = await context.Periods
                .Select(period => new
                {
                    period.PeriodId,
                    period.IsActive,
                    period.StartPeriodDate,
                    period.EndPeriodDate
                })
                .OrderBy(period => period.StartPeriodDate)
                .ToListAsync();

            var selectedPeriodId = periods.FirstOrDefault(p => p.IsActive)?.PeriodId;

            ViewBag.Periods = new SelectList(
                periods.Select(p => new
                {
                    p.PeriodId,
                    DisplayText = $"{p.StartPeriodDate:dd MMM yyyy} - {p.EndPeriodDate:dd MMM yyyy}"
                }),
                "PeriodId",
                "DisplayText",
                selectedPeriodId
            );
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
            return View(new EditViewModel
            {
                PeriodId = medicalClaim.PeriodId,
                ClaimDate = medicalClaim.ClaimDate,
                EmployeeId = medicalClaim.EmployeeId,
                Diagnosis = medicalClaim.Diagnosis ?? "",
                PaymentPeriod = medicalClaim.PaymentPeriod,
                ClaimStatus = medicalClaim.ClaimStatus ?? "",
                ClaimCategory = medicalClaim.ClaimCategory ?? "",
                ClaimDescription = medicalClaim.ClaimDescription ?? "",
                EmployeeMedicalClaimId = medicalClaim.EmployeeMedicalClaimId,
                EmployeeMedicalClaimCode = medicalClaim.EmployeeMedicalClaimCode ?? ""
            });
        }

        // POST: ManagementClaimController/Edit/5
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
                    return RedirectToAction("Logout", "Account");
                }

                if (ModelState.IsValid)
                {
                    var oldMedicalClaim = await context.EmployeeMedicalClaims.FindAsync(id);
                    var logs = new AppLogModel
                    {
                        CreatedBy = $"{user.Id} - {user.FullName}",
                        CreatedAt = time,
                        Params = JsonConvert.SerializeObject(new
                        {
                            oldData = JsonConvert.SerializeObject(new
                            {
                                oldMedicalClaim.EmployeeMedicalClaimCode,
                                oldMedicalClaim.EmployeeMedicalClaimId,
                                oldMedicalClaim.ClaimCategory,
                                oldMedicalClaim.ClaimDate,
                                oldMedicalClaim.ClaimDescription,
                                oldMedicalClaim.ClaimStatus,
                                oldMedicalClaim.Diagnosis,
                                oldMedicalClaim.EmployeeId,
                                oldMedicalClaim.PaymentPeriod,
                                oldMedicalClaim.PeriodId,
                                oldMedicalClaim.CreatedAt,
                                oldMedicalClaim.UpdatedAt,
                                oldMedicalClaim.CreatedBy,
                                oldMedicalClaim.UpdatedBy
                            }),
                            newData = model
                        }),
                        Source = JsonConvert.SerializeObject(new
                        {
                            Controller = "ManagementMedicalClaim",
                            Action = "Edit",
                            Database = "EmployeeMedicalClaims"
                        })
                    };

                    //update data
                    oldMedicalClaim.EmployeeMedicalClaimCode = model.EmployeeMedicalClaimCode;
                    oldMedicalClaim.ClaimCategory = model.ClaimCategory;
                    oldMedicalClaim.ClaimDate = model.ClaimDate;
                    oldMedicalClaim.ClaimDescription = model.ClaimDescription;
                    oldMedicalClaim.ClaimStatus = model.ClaimStatus;
                    oldMedicalClaim.Diagnosis = model.Diagnosis;
                    oldMedicalClaim.EmployeeId = model.EmployeeId;
                    oldMedicalClaim.PaymentPeriod = model.PaymentPeriod;
                    oldMedicalClaim.PeriodId = model.PeriodId;
                    oldMedicalClaim.UpdatedAt = time;
                    oldMedicalClaim.UpdatedBy = user.Id;
                    context.EmployeeMedicalClaims.Update(oldMedicalClaim);
                    await context.SaveChangesAsync();
                    await mongoDbContext.AppLogs.InsertOneAsync(logs);
                    TempData["Success"] = "Data has been updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                var medicalClaim = await context.EmployeeMedicalClaims.FindAsync(id);
                if (medicalClaim == null)
                {
                    TempData["Errors"] = "Data not found";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Categories = new List<string> { "RAWAT_JALAN", "RAWAT_INAP", "MELAHIRKAN", "KACAMATA" };
                var periods = await context.Periods
                    .Select(period => new
                    {
                        period.PeriodId,
                        period.IsActive,
                        period.StartPeriodDate,
                        period.EndPeriodDate
                    })
                    .OrderBy(period => period.StartPeriodDate)
                    .ToListAsync();

                var selectedPeriodId = periods.FirstOrDefault(p => p.IsActive)?.PeriodId;

                ViewBag.Periods = new SelectList(
                    periods.Select(p => new
                    {
                        p.PeriodId,
                        DisplayText = $"{p.StartPeriodDate:dd MMM yyyy} - {p.EndPeriodDate:dd MMM yyyy}"
                    }),
                    "PeriodId",
                    "DisplayText",
                    selectedPeriodId
                );
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
                return View(model);
            }
            catch (Exception e)
            {
                TempData["Errors"] = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ManagementClaimController/Import
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(ImportMedicalClaimViewModel model)
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

                var claims = await ReadMedicalClaimsFromExcelAsync(model.ExcelFile, user.Id);

                if (claims.Count != 0)
                {
                    await medicalClaimService.InsertBatchMedicalClaimsAsync(claims);
                    TempData["Success"] = "Data has been imported successfully";
                }
                else
                {
                    TempData["Errors"] = "No valid data found to import";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Errors"] = $"Error during import: {ex.Message}";
                return View(model);
            }
        }


        [HttpGet]
        [Authorize(Roles = "ClaimManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var medicalClaim = await context.EmployeeMedicalClaims.Include(emp => emp.Employee)
                .ThenInclude(usr => usr.Users).Where(mdc => mdc.EmployeeMedicalClaimId == id)
                .Select(mdc => new EmployeeMedicalClaim
                {
                    EmployeeMedicalClaimId = mdc.EmployeeMedicalClaimId,
                    ClaimDate = mdc.ClaimDate,
                    ClaimStatus = mdc.ClaimStatus,
                    ClaimCategory = mdc.ClaimCategory,
                    ClaimDescription = mdc.ClaimDescription,
                    Diagnosis = mdc.Diagnosis,
                    PaymentPeriod = mdc.PaymentPeriod,
                    PeriodId = mdc.PeriodId,
                    EmployeeId = mdc.EmployeeId,
                    Employee = new EmployeeModel
                    {
                        Nip = mdc.Employee.Nip,
                        Users = new Users
                        {
                            FullName = mdc.Employee.Users.FullName
                        }
                    },
                    Period = new PeriodModel
                    {
                        StartPeriodDate = mdc.Period.StartPeriodDate,
                        EndPeriodDate = mdc.Period.EndPeriodDate
                    }
                }).FirstOrDefaultAsync();
            return View(medicalClaim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ClaimManager")]
        public async Task<IActionResult> Delete(int id, IFormCollection model)
        {
            try
            {
                var reason = model["DeleteReason"].FirstOrDefault();
                if (string.IsNullOrEmpty(reason))
                {
                    ModelState.AddModelError("DeleteReason", "Delete Reason is Required");
                    var medicalClaim = await context.EmployeeMedicalClaims.Include(emp => emp.Employee)
                        .ThenInclude(usr => usr.Users).Where(mdc => mdc.EmployeeMedicalClaimId == id)
                        .Select(mdc => new EmployeeMedicalClaim
                        {
                            EmployeeMedicalClaimId = mdc.EmployeeMedicalClaimId,
                            ClaimDate = mdc.ClaimDate,
                            ClaimStatus = mdc.ClaimStatus,
                            ClaimCategory = mdc.ClaimCategory,
                            ClaimDescription = mdc.ClaimDescription,
                            Diagnosis = mdc.Diagnosis,
                            PaymentPeriod = mdc.PaymentPeriod,
                            PeriodId = mdc.PeriodId,
                            EmployeeId = mdc.EmployeeId,
                            Employee = new EmployeeModel
                            {
                                Nip = mdc.Employee.Nip,
                                Users = new Users
                                {
                                    FullName = mdc.Employee.Users.FullName
                                }
                            },
                            Period = new PeriodModel
                            {
                                StartPeriodDate = mdc.Period.StartPeriodDate,
                                EndPeriodDate = mdc.Period.EndPeriodDate
                            }
                        }).FirstOrDefaultAsync();
                    return View(medicalClaim);
                }

                var user = await userManager.GetUserAsync(User);
                var time = DateTime.UtcNow;
                if (user == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                var oldData = await context.EmployeeMedicalClaims.FindAsync(id);
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
                        oldData.ClaimDate,
                        oldData.ClaimCategory,
                        oldData.ClaimDescription,
                        oldData.ClaimStatus,
                        oldData.EmployeeId,
                        oldData.PaymentPeriod,
                        oldData.Diagnosis,
                        oldData.UpdatedAt,
                        oldData.UpdatedBy,
                        oldData.PeriodId,
                        oldData.EmployeeMedicalClaimCode,
                        oldData.EmployeeMedicalClaimId,
                        DeleteReason = reason
                    }),
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "ManagementMedicalClaim",
                        Action = "Delete",
                        Database = "EmployeeMedicalClaim"
                    })
                };
                oldData.DeleteReason = reason;
                oldData.IsDeleted = true;
                oldData.UpdatedAt = time;
                oldData.UpdatedBy = user.Id;
                context.EmployeeMedicalClaims.Update(oldData);
                await context.SaveChangesAsync();
                await mongoDbContext.AppLogs.InsertOneAsync(logs);
                TempData["Success"] = "Data has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["Errors"] = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetMedicalClaims()
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Convert.ToInt32(HttpContext.Request.Form["start"].FirstOrDefault());
            var length = Convert.ToInt32(HttpContext.Request.Form["length"].FirstOrDefault());
            var searchValue = HttpContext.Request.Form["search[value]"].FirstOrDefault();
            var sortColumnIndex = Convert.ToInt32(HttpContext.Request.Form["order[0][column]"].FirstOrDefault());
            var sortColumnDirection = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
            sortColumnDirection = (sortColumnDirection == "desc") ? "desc" : "asc";

            string[] columnNames =
            {
                "EmployeeMedicalClaimCode",
                "EmployeeMedicalClaimId",
                "Employee.Nip",
                "Employee.Users.FullName",
                "ClaimDate",
                "ClaimCategory"
            };

            var query = context.EmployeeMedicalClaims
                .Include(emc => emc.Period)
                .Include(emc => emc.Employee)
                .ThenInclude(e => e.Users)
                .Where(emc => !emc.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(emc =>
                    emc.Employee != null && emc.Employee.Users != null && emc.ClaimStatus != null &&
                    emc.Diagnosis != null && emc.ClaimCategory != null &&
                    (emc.Employee.Nip.ToString().Contains(searchValue) ||
                     emc.EmployeeMedicalClaimCode.Contains(searchValue) ||
                     emc.Employee.Users.FullName.Contains(searchValue) ||
                     emc.Diagnosis.Contains(searchValue) ||
                     emc.Employee.Users.FullName.Contains(searchValue) ||
                     emc.ClaimCategory.Contains(searchValue))
                );
            }

            var totalRecords = await query.CountAsync();

            if (sortColumnIndex >= 0 && sortColumnIndex < columnNames.Length)
            {
                string sortColumn = columnNames[sortColumnIndex];
                if (sortColumn == "EmployeeMedicalClaimId")
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(emc => emc.EmployeeMedicalClaimId)
                        : query.OrderByDescending(emc => emc.EmployeeMedicalClaimId);
                else if (sortColumn == "Employee.Nip")
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(emc => emc.Employee.Nip)
                        : query.OrderByDescending(emc => emc.Employee.Nip);
                else if (sortColumn == "Employee.Users.FullName")
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(emc => emc.Employee.Users.FullName)
                        : query.OrderByDescending(emc => emc.Employee.Users.FullName);
                else if (sortColumn == "ClaimDate")
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(emc => emc.ClaimDate)
                        : query.OrderByDescending(emc => emc.ClaimDate);
                else if (sortColumn == "ClaimCategory")
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(emc => emc.ClaimCategory)
                        : query.OrderByDescending(emc => emc.ClaimCategory);
                else if (sortColumn == "EmployeeMedicalClaimCode")
                {
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(emc => emc.EmployeeMedicalClaimCode)
                        : query.OrderByDescending(emc => emc.EmployeeMedicalClaimCode);
                }
            }

            // Pagination
            var pagedData = await query
                .Skip(start)
                .Take(length)
                .Select(emc => new
                {
                    emc.EmployeeMedicalClaimCode,
                    emc.EmployeeMedicalClaimId,
                    StartEndPeriod = emc.Period.StartPeriodDate.ToString("dd/MMM/yyyy") + " - " +
                                     emc.Period.EndPeriodDate.ToString("dd/MMM/yyyy"),
                    emc.Employee.Nip,
                    EmployeeName = emc.Employee.Users.FullName,
                    ClaimDate = emc.ClaimDate.HasValue ? emc.ClaimDate.Value.ToString("dd/MMM/yyyy") : "",
                    emc.ClaimStatus,
                    emc.ClaimCategory
                })
                .ToListAsync();
            var response = new
            {
                draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = pagedData
            };
            return Json(response);
        }

        private async Task<List<object>> ReadMedicalClaimsFromExcelAsync(IFormFile excelFile, string user)
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
                            Period = reader.GetValue(1)?.ToString(),
                            NIP = int.TryParse(reader.GetValue(2)?.ToString(), out var emplId) ? emplId : 0,
                            Code = reader.GetValue(4)?.ToString(),
                            ClaimCategory = reader.GetValue(6)?.ToString(),
                            ClaimDate = ParseDate(reader.GetValue(7)?.ToString()),
                            Diagnosis = reader.GetValue(8)?.ToString(),
                            ClaimStatus = reader.GetValue(11)?.ToString(),
                            PaymentPeriod = ParseDate(reader.GetValue(12)?.ToString()),
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