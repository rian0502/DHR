using DHR.Helper;
using DHR.Models;
using DHR.Providers;
using DHR.ViewModels.Education;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DHR.Controllers;

[Authorize(Roles = "Admin")]
public class EducationController(EducationService educationService, UserManager<Users> userManager, MongoDbContext mongoDbContext) : Controller
{
    // GET: EducationController
    public async Task<ActionResult> Index()
    {
        var educations = await educationService.FindAll();
        return View(educations);
    }
    
    // GET: EducationController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: EducationController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateEducationViewModel model)
    {
        try
        {
            if (!ModelState.IsValid) return View(model);
            var user = await userManager.GetUserAsync(User);
            var currentTime = DateTime.UtcNow;
            var insert = await educationService.Insert(model, user!.Id, currentTime);
            if (insert == 0)
            {
                TempData["Errors"] = "Failed to create Education";
                return View(model);
            }

            var logs = new AppLogModel()
            {
                CreatedBy = $"{user.Id} - {user.FullName}",
                CreatedAt = currentTime,
                Params = JsonConvert.SerializeObject(new
                {
                    model.EducationName
                }),
                Source =JsonConvert.SerializeObject(new
                {
                    Controller = "EducationController",
                    Action = "Create",
                    Database = "Educations"
                }),
            };
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Education created successfully";
            return RedirectToAction(nameof(Index));
        }
        catch(Exception ex)
        {
            TempData["Errors"] = ex.Message;
            return View(model);
        }
    }

    // GET: EducationController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        var result = await educationService.FindById(id);
        return View(new EditEducationViewModel{
            EducationId = result.EducationId,
            EducationName = result.EducationName ?? ""
        });
    }

    // POST: EducationController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, EditEducationViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.GetUserAsync(User);
            var currentTime = DateTime.UtcNow;
            var oldData = await educationService.FindById(id);
            var update = await educationService.Update(model, user!.Id, currentTime);
            if (update == 0)
            {
                TempData["Errors"] = "Failed to update Education";
                return View(model);
            }

            var logs = new AppLogModel()
            {
                CreatedBy = $"{user.Id} - {user.FullName}",
                CreatedAt = currentTime,
                Params = JsonConvert.SerializeObject(new
                {
                    OldData = oldData,
                    NewData = model
                }),
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "EducationController",
                    Action = "Edit",
                    Database = "Educations"
                }),
            };
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Education updated successfully";
            return RedirectToAction(nameof(Index));
        }
        catch(Exception ex)
        {
            TempData["Errors"] = ex.Message;
            return View(model);
        }
    }
    
    // POST: EducationController/Delete/5
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
           return RedirectToAction(nameof(Index));
        }
    }
}