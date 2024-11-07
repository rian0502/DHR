using Microsoft.AspNetCore.Mvc;
using Presensi360.Services;

namespace Presensi360.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly CompanyServices _companyServices;
        private readonly LocationServices _locationServices;

        public CompaniesController(CompanyServices companyServices, LocationServices locationServices)
        {
            _companyServices = companyServices;
            _locationServices = locationServices;
        }
        public async Task<IActionResult> Index()
        {
            var companies = await _companyServices.FindAll();
            return View(companies);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var company = await _companyServices.FindById(id);

            if (company == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var locations = await _locationServices.FindAll();
            ViewData["Locations"] = locations;

            return View(company);
        }
    }
}
