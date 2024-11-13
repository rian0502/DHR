using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presensi360.Providers;
using Presensi360.ViewModels;


namespace Presensi360.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private readonly CompanyService _companyService;
        private readonly WorkAreaService _workAreaService;
        public CompaniesController(CompanyService companyService, WorkAreaService workAreaService)
        {
            _companyService = companyService;
            _workAreaService = workAreaService;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await _companyService.FindAll();
        
            return View(companies);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.workAreas = await _workAreaService.FindAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _companyService.Insert(model);
                if (result == 3)
                {
                    TempData["Errors"] = "Company Code already exists";
                }else if (result == 0)
                {
                    TempData["Errors"] = "Failed to create company";
                }else
                {
                    TempData["Success"] = "Company created successfully";
                }
                return RedirectToAction("Index");
            }
            ViewBag.WorkAreas = await _workAreaService.FindAll();
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _companyService.FindById(id);
            ViewBag.WorkAreas = await _workAreaService.FindAll();
            EditCompanyViewModel model = new EditCompanyViewModel
            {
                CompanyID = result.CompanyID,
                CompanyCode = result.CompanyCode,
                CompanyName = result.CompanyName,
                LocationID = result.LocationID,
            };
            return View(model);
        }

        public async Task<IActionResult> Edit(EditCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _companyService.Update(model);
                if (result == 0)
                {
                    TempData["Errors"] = "Failed to update company";
                }
                else
                {
                    TempData["Success"] = "Company updated successfully";
                }
                return RedirectToAction("Index");
            }
            ViewBag.WorkAreas = await _workAreaService.FindAll();
            return View(model);
        }
    }
}
