using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Presensi360.Providers;
using Presensi360.ViewModels;

namespace Presensi360.Controllers
{
    [Authorize]
    public class JobTitlesController : Controller
    {
        private readonly JobTitleService _jobTitleService;

        public JobTitlesController(JobTitleService jobTitleService)
        {
            _jobTitleService = jobTitleService;
        }

        public async Task<IActionResult> Index()
        {
            var jobTitles = await _jobTitleService.FindAll();
            return View(jobTitles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobTitleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var insert = await _jobTitleService.Insert(model);
                if (insert == 3)
                {
                    TempData["Errors"] = "Job Title Code already exist";
                }
                else if (insert == 0)
                {
                    TempData["Errors"] = "Failed to create Job Title";
                }
                else
                {
                    TempData["Success"] = "Job Title has been created";
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var jobTitle = await _jobTitleService.FindById(id);
            EditJobTitleViewModel model = new()
            {
                JobTitleID = jobTitle.JobTitleID,
                JobTitleName = jobTitle.JobTitleName,
                JobTitleCode = jobTitle.JobTitleCode,
                JobTitleDescription = jobTitle.JobTitleDescription
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditJobTitleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var update = await _jobTitleService.Update(model);

                if(update == 0)
                {
                    TempData["Errors"] = "Failed to update Job Title";
                }
                else
                {
                    TempData["Success"] = "Job Title has been updated";
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
