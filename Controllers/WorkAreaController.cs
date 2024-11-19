using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presensi360.Helper;
using Presensi360.Models;
using Presensi360.Providers;
using Presensi360.ViewModels;
using System.Security.Claims;

namespace Presensi360.Controllers
{
    [Authorize]
    public class WorkAreaController : Controller
    {
        private readonly WorkAreaService _workAreaService;
        private readonly MongoDBContext _mongoDBContext;
        private readonly UserManager<Users> _userManager;
        public WorkAreaController(WorkAreaService workAreaService, MongoDBContext mongoDBContext, UserManager<Users> userManager)
        {
            _workAreaService = workAreaService;
            _mongoDBContext = mongoDBContext;
            _userManager = userManager;
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
                var user = await _userManager.GetUserAsync(User);
                var insert = await _workAreaService.Create(model, user!.Id);
                if(insert == 0)
                {
                    TempData["Errors"] = "Failed to create Work Area";
                    return View(model);
                }
                else
                {
                    var Logs = new AppLogModel()
                    {
                        Params = JsonConvert.SerializeObject(new
                        {
                            LocationCode = model.LocationCode,
                            LocationName = model.LocationName,
                        }),
                        Source = JsonConvert.SerializeObject(new
                        {
                            Controller = "WorkAreaController",
                            Action = "Create",
                        }),
                        CreatedBy = $"{user!.Id} - {user.FullName}",
                        CreatedAt = DateTime.UtcNow
                    };
                    await _mongoDBContext.AppLogs.InsertOneAsync(Logs);

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
