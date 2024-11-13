using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presensi360.Helper;
using Presensi360.Models;
using Presensi360.ViewModels;
using System.Transactions;

namespace Presensi360.Controllers
{
    public class AccountController : Controller
    {

        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;
        private readonly AppDBContext _context;

        public AccountController  (SignInManager<Users> signInManager, UserManager<Users> userManager, AppDBContext appDBContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = appDBContext;
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false).Result;

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user != null)
                    { 
                        var employee = await _context.Employee.FirstOrDefaultAsync(e => e.UserId == user.Id);
                        
                    }
                    return RedirectToAction("Dashboard", "Home");
                }
                TempData["Errors"] = "Invalid Login Attempt";
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
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

    }
}
