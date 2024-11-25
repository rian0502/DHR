using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.Providers;
using DAHAR.ViewModels.JobTitle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class JobTitleController(
    JobTitleService jobTitleService,
    UserManager<Users> userManager,
    MongoDBContext mongoDbContext) : Controller
{
    public async Task<IActionResult> Index()
    {
        var jobTitles = await jobTitleService.FindAll();
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
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            var currentTime = DateTime.UtcNow;
            var insert = await jobTitleService.Insert(model, user!.Id, currentTime);
            switch (insert)
            {
                case 3:
                    TempData["Errors"] = "Job Title Code already exist";
                    break;
                case 0:
                    TempData["Errors"] = "Failed to create Job Title";
                    break;
                default:
                    var logs = new AppLogModel
                    {
                        CreatedBy = $"{user.Id} - {user.FullName}",
                        CreatedAt = currentTime,
                        Params = JsonConvert.SerializeObject(new
                        {
                            model.JobTitleCode,
                            model.JobTitleName,
                            model.JobTitleDescription
                        }),
                        Source = JsonConvert.SerializeObject(new
                        {
                            Controller = "JobTitleController",
                            Action = "Create",
                            Database = "JobTitle",
                        })
                    };
                    await mongoDbContext.AppLogs.InsertOneAsync(logs);
                    TempData["Success"] = "Job Title has been created";
                    break;
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var jobTitle = await jobTitleService.FindById(id);
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
    public async Task<IActionResult> Edit(int id, EditJobTitleViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            var oldData = await jobTitleService.FindById(id);
            var currentTime = DateTime.UtcNow;
            var update = await jobTitleService.Update(model, user!.Id, currentTime);

            if (update == 0)
            {
                TempData["Errors"] = "Failed to update Job Title";
            }
            else
            {
                var logs = new AppLogModel
                {
                    CreatedBy = $"{user.Id} - {user.FullName}",
                    CreatedAt = currentTime,
                    Params = JsonConvert.SerializeObject(new
                    {
                        oldData = JsonConvert.SerializeObject(oldData),
                        newData = JsonConvert.SerializeObject(model)
                    }),
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "JobTitleController",
                        Action = "Edit",
                        Database = "JobTitle",
                    })
                };
                await mongoDbContext.AppLogs.InsertOneAsync(logs);
                TempData["Success"] = "Job Title has been updated";
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}