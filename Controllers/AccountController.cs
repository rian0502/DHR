using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DHR.Helper;
using DHR.Models;
using DHR.ViewModels;
using DHR.ViewModels.Employee;
using DHR.ViewModels.Profile;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using AppLogModel = DHR.Models.AppLogModel;

namespace DHR.Controllers;

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

        var user = await userManager.FindByNameAsync(model.Username);

        if (user == null)
        {
            TempData["Errors"] = "Invalid Login Attempt";
            return View(model);
        }

        if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow)
        {
            TempData["Errors"] = "Your account is locked. Please contact the Vendor!.";
            return View(model);
        }

        var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);

        if (result.Succeeded)
        {
            await appDbContext.Employee.FirstOrDefaultAsync(e => e.UserId == user.Id);
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

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await appDbContext.Employee.Include(usr => usr.Users)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);
        if (user == null || user.Users == null)
        {
            TempData["Errors"] = "Employee Not Found";
        }

        return View(new EditAccountEmployeeViewModel
        {
            UserID = user.UserId,
            Email = user.Users.Email,
            Username = user.Users.UserName,
            LockoutEnd = user.Users.LockoutEnd
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, EditAccountEmployeeViewModel model)
    {
        try
        {
            var admin = await userManager.GetUserAsync(User);
            var time = DateTime.UtcNow;
            if (admin == null)
            {
                return RedirectToAction("Logout");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await appDbContext.Users.FirstOrDefaultAsync(usr => model.UserID == usr.Id);
            if (user == null)
            {
                TempData["Errors"] = "Employee Not Found";
                return View(model);
            }

            var logs = new AppLogModel
            {
                CreatedBy = $"{admin.Id} - {admin.FullName}",
                CreatedAt = time,
                Source = JsonConvert.SerializeObject(new
                {
                    Controller = "AccountController",
                    Action = "Edit",
                    Database = "AspNetUsers"
                }),
                Params = JsonConvert.SerializeObject(new
                {
                    newData = JsonConvert.SerializeObject(new
                    {
                        model.Email,
                        model.LockoutEnd,
                        model.Username
                    }),
                    oldData = JsonConvert.SerializeObject(new
                    {
                        user.Email,
                        user.LockoutEnd,
                        user.UserName
                    })
                })
            };

            user.Email = model.Email;
            user.LockoutEnd = model.LockoutEnd;
            user.UserName = model.Username;
            user.NormalizedUserName = model.Username.ToUpper();

            if (!model.Password.IsNullOrEmpty())
            {
                var remove = await userManager.RemovePasswordAsync(user);
                var password = model.Password;

                var add = await userManager.AddPasswordAsync(user, password);
                if (remove.Succeeded && add.Succeeded)
                {
                    await appDbContext.SaveChangesAsync();
                    logs.Params = JsonConvert.SerializeObject(new
                    {
                        newData = JsonConvert.SerializeObject(new
                        {
                            model.Email,
                            model.LockoutEnd,
                            model.Username,
                        }),
                        oldData = JsonConvert.SerializeObject(new
                        {
                            user.Email,
                            user.LockoutEnd,
                            user.UserName
                        }),
                        password = "Password Reset"
                    });
                    await mongoDbContext.AppLogs.InsertOneAsync(logs);
                    TempData["Success"] = "Account Updated with Password Reset Successfully";
                    return RedirectToAction("Index", "Employee");
                }

                TempData["Errors"] = "Password Reset Failed";
                return View(model);
            }

            await appDbContext.SaveChangesAsync();
            await mongoDbContext.AppLogs.InsertOneAsync(logs);
            TempData["Success"] = "Account Updated Successfully";
            return RedirectToAction("Index", "Employee");
        }
        catch (Exception e)
        {
            TempData["Errors"] = e.Message;
            return View(model);
        }
    }
}