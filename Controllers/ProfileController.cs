using DHR.Helper;
using DHR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DHR.Controllers;

[Authorize(Roles = "User")]
public class ProfileController(UserManager<Users> userManager, AppDbContext context) : Controller
{
    // GET: ProfileController
    public async Task<ActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        var profile = await context.Employee
            .Include(e => e.Users)
            .Include(e => e.SubUnit)
            .Include(e => e.JobTitle)
            .Include(e => e.Education)
            .Include(e => e.Religion)
            .Include(e => e.Division)
            .ThenInclude(d => d!.SubDepartment)
            .ThenInclude(de => de.Department)
            .ThenInclude(c => c!.Company)
            .Include(e => e.TaxExemptIncome)
            .Include(e => e.EmployeeDependents)
            .Include(be => be.Benefits)
            .ThenInclude(b => b.Benefit)
            .FirstOrDefaultAsync(e => e.Users!.Id == user!.Id);
        return View(profile);
    }

    // GET: ProfileController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: ProfileController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: ProfileController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: ProfileController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: ProfileController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: ProfileController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: ProfileController/Delete/5
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
            return View();
        }
    }
}