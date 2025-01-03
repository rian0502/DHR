﻿using DHR.Helper;
using DHR.Providers;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DHR.ViewModels.ManagementImportViewModel;

namespace DHR.Controllers
{
    public class ManagementMedicalClaimController(
        AppDbContext context,
        MongoDbContext mongoDbContext,
        MedicalClaimService medicalClaimService) : Controller
    {
        // GET: ManagementClaimController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ManagementClaimController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManagementClaimController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ManagementClaimController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManagementClaimController/Edit/5
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

        // GET: ManagementClaimController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManagementClaimController/Delete/5
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
                var claims = await ReadMedicalClaimsFromExcelAsync(model.ExcelFile);

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

            // Kolom yang digunakan untuk sorting (sesuaikan urutannya dengan urutan di DataTable)
            string[] columnNames =
            {
                "EmployeeMedicalClaimId",
                "Employee.Nip",
                "Employee.Users.FullName",
                "StartEndPeriod",
                "ClaimDate",
                "ClaimStatus",
                "ClaimCategory",
                "Period.StartPeriodDate",
                "Period.EndPeriodDate"
            };

            var query = context.EmployeeMedicalClaims
                .Include(emc => emc.Period)
                .Include(emc => emc.Employee)
                .ThenInclude(e => e.Users)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(emc =>
                    emc.Employee != null && emc.Employee.Users != null && emc.ClaimStatus != null &&
                    emc.Diagnosis != null && emc.ClaimCategory != null &&
                    (emc.Employee.Nip.ToString().Contains(searchValue) ||
                     emc.Employee.Users.FullName.Contains(searchValue) ||
                     emc.ClaimStatus.Contains(searchValue) ||
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
                else if (sortColumn == "ClaimStatus")
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(emc => emc.ClaimStatus)
                        : query.OrderByDescending(emc => emc.ClaimStatus);
                else if (sortColumn == "ClaimCategory")
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(emc => emc.ClaimCategory)
                        : query.OrderByDescending(emc => emc.ClaimCategory);
                else if (sortColumn == "StartEndPeriod")
                {
                    query = sortColumnDirection == "asc"
                        ? query.OrderBy(emc => emc.Period.StartPeriodDate).ThenBy(emc => emc.Period.EndPeriodDate)
                        : query.OrderByDescending(emc => emc.Period.StartPeriodDate)
                            .ThenByDescending(emc => emc.Period.EndPeriodDate);
                }
            }

            // Pagination
            var pagedData = await query
                .Skip(start)
                .Take(length)
                .Select(emc => new
                {
                    emc.EmployeeMedicalClaimId,
                    StartEndPeriod = emc.Period.StartPeriodDate.ToString("dd/MMM/yyyy") + " - " +
                                     emc.Period.EndPeriodDate.ToString("dd/MMM/yyyy"),
                    Nip = emc.Employee.Nip,
                    EmployeeName = emc.Employee.Users.FullName,
                    ClaimDate = emc.ClaimDate.HasValue ? emc.ClaimDate.Value.ToString("dd/MMM/yyyy") : "",
                    ClaimStatus = emc.ClaimStatus,
                    ClaimCategory = emc.ClaimCategory
                })
                .ToListAsync();
            var response = new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = pagedData
            };
            return Json(response);
        }

        private async Task<List<object>> ReadMedicalClaimsFromExcelAsync(IFormFile excelFile)
        {
            var claims = new List<object>();

            await using var stream = excelFile.OpenReadStream();
            using var reader = ExcelReaderFactory.CreateReader(stream);
            bool isFirstSheet = true;
            do
            {
                if (isFirstSheet)
                {
                    reader.Read(); // Skip the header row

                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0) || reader.IsDBNull(1)) continue;

                        var claim = new
                        {
                            EmployeeName = reader.GetValue(1)?.ToString(),
                            EmployeeId = int.TryParse(reader.GetValue(2)?.ToString(), out var emplId) ? emplId : 0,
                            ClaimDate = ParseDate(reader.GetValue(6)?.ToString()),
                            Description = reader.GetValue(5)?.ToString(),
                            Diagnosis = reader.GetValue(7)?.ToString(),
                            TreatmentType = reader.GetValue(10)?.ToString(),
                            PaymentPeriod = ParseDate(reader.GetValue(11)?.ToString()),
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
                : (DateOnly?)null;
        }
    }
}