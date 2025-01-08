using DHR.Helper;
using DHR.Models;
using DHR.ViewModels.FormApplicationRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DHR.Controllers;

[Authorize(Roles = "Admin")]
public class FormApplicationRequestController(
    AppDbContext context,
    UserManager<Users> userManager,
    MongoDbContext mongoDbContext) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var forms = await context.FormApplication.ToListAsync();
        return View(forms);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var form = await context.FormApplication.FindAsync(id);
        if (form != null)
        {
            return View(new EditFormApplicationRequest
            {
                IdForm = form.FormId,
                FormCode = form.FormCode ?? "",
                FormName = form.FormName ?? "",
                Description = form.Description ?? "",
                FormPath = form.PathForm ?? ""
            });
        }
        TempData["Error"] = "Form Application Not Found";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditFormApplicationRequest model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await userManager.GetUserAsync(User);
        var currentTime = DateTime.UtcNow;
        if (user == null)
        {
            return RedirectToAction("Logout", "Account");
        }
        var form = await context.FormApplication.FindAsync(id);
        if (form == null)
        {
            return NotFound();
        }
        var oldData = form;
        form.FormCode = model.FormCode;
        form.FormName = model.FormName;
        form.Description = model.Description;
        form.UpdatedBy = user.Id;

        if (model.File != null)
        {
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/TemplateForm");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            if (!string.IsNullOrEmpty(form.PathForm))
            {
                var oldFilePath = Path.Combine(uploadPath, form.PathForm);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(model.File.FileName)}";
            var newFilePath = Path.Combine(uploadPath, newFileName);
            await using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }
            form.PathForm = newFileName;
        }
        await context.SaveChangesAsync();
        var logs = new AppLogModel
        {
            CreatedAt = currentTime,
            CreatedBy = $"{user.Id} - {user.FullName}",
            Params = JsonConvert.SerializeObject(new
            {
                OldData = oldData,
                NewData = form
            }),
            Source = JsonConvert.SerializeObject(new
            {
                Controller = "FormApplicationRequestController",
                Action = "Edit",
                Database = "FormApplication"
            })
        };
        await mongoDbContext.AppLogs.InsertOneAsync(logs);
        TempData["Success"] = "Form Application Updated Successfully";
        return RedirectToAction(nameof(Index));
    }
}