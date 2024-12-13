using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.ViewModels;
using DAHAR.ViewModels.Profile;
using Newtonsoft.Json;

namespace DAHAR.Controllers;

public class AccountController(
    SignInManager<Users> signInManager,
    UserManager<Users> userManager,
    MongoDbContext mongoDbContext,
    AppDbContext appDbContext)
    : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return User.Identity?.IsAuthenticated == true ? RedirectToAction("Dashboard", "Home") : View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false)
            .Result;

        if (result.Succeeded)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                await appDbContext.Employee.FirstOrDefaultAsync(e => e.UserId == user.Id);
            }

            return RedirectToAction("Dashboard", "Home");
        }

        TempData["Errors"] = "Invalid Login Attempt";
        return View(model);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult VerifyEmail()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email Not Found");
                return View(model);
            }

            return RedirectToAction("ResetPassword", new { model.Email });
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPassword(string email)
    {
        return View(new ResetPasswordViewModel { Email = email });
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("Email", "Email Not Found");
            return View(model.Email);
        }

        var remove = await userManager.RemovePasswordAsync(user);
        var password = model.Password;
        if (password != null)
        {
            var add = await userManager.AddPasswordAsync(user, password);
            if (remove.Succeeded && add.Succeeded)
            {
                TempData["Success"] = "Password Reset Successfully";
                return RedirectToAction("Login", "Account");
            }
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Ok(new
            {
                success = false,
                errors
            });
        }

        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Ok(new { success = false, errors = new List<string> { "User Not Found" } });
        }

        var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Ok(new { success = false, errors });
        }

        var logs = new AppLogModel
        {
            CreatedBy = $"{user.Id} - {user.FullName}",
            CreatedAt = DateTime.UtcNow,
            Source = JsonConvert.SerializeObject(new
            {
                Controller = "AccountController",
                Action = "ChangePassword",
                Database = "AspNetUsers"
            })
        };
        await mongoDbContext.AppLogs.InsertOneAsync(logs);
        return Ok(new { success = true });
    }

    public IActionResult AccessDenied()
    {
        return RedirectToAction("Dashboard", "Home");
    }
}