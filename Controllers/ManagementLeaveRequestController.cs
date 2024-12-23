using DHR.Providers;
using DHR.ViewModels.ManagementImportViewModel;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DHR.Controllers
{
    public class ManagementLeaveRequestController(LeaveRequestService leaveRequestService) : Controller
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManagementLeaveRequest/Create
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
                            LeaveDays = double.TryParse(reader.GetValue(4)?.ToString().Replace(",", "."), out var days) ? days : 0,
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
