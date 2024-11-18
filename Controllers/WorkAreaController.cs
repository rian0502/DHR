using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presensi360.Providers;
using Presensi360.ViewModels;

namespace Presensi360.Controllers
{
    [Authorize]
    public class WorkAreaController : Controller
    {
        private readonly WorkAreaService _workAreaService;

        public WorkAreaController(WorkAreaService workAreaService)
        {
            _workAreaService = workAreaService;
        }
        public async Task<IActionResult> Index()
        {
            var locations = await _workAreaService.FindAll();
            return View(locations);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateWorkAreaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var insert = await _workAreaService.Create(model);
                if(insert == 0)
                {
                    TempData["Errors"] = "Failed to create Work Area";
                    return View(model);
                }
                else
                {
                    TempData["Success"] = "Work Area has ben created";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _workAreaService.FindById(id);
            EditWorkAreaViewModel model = new()
            {
                LocationID = result.LocationID,
                LocationCode = result.LocationCode,
                LocationName = result.LocationName
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditWorkAreaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _workAreaService.Update(model);
                if(result == 0)
                {
                    TempData["Errors"] = "Failed to create Work Area";
                    return View(model);
                }
                else
                {
                    TempData["Success"] = "Work Area has ben updated";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
    }
}
