using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.ViewModels;

namespace DAHAR.Controllers
{
    public class AccountController(
        SignInManager<Users> signInManager,
        UserManager<Users> userManager,
        AppDBContext appDbContext)
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

            if (ModelState.IsValid)
            {
                var result = signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false).Result;

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
            }
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
        public IActionResult VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("ResetPassword", new { model.Email});
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            return View(new ResetPasswordViewModel { Email = email} );
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(IFormCollection model)
        {
            return Ok(model);
        }


        public IActionResult AccessDenied()
        {
            return RedirectToAction("Dashboard", "Home");
        }
    }
}
