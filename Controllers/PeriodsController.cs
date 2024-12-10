using DAHAR.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class PeriodsController(PeriodService periodService) : Controller
{
    // GET: PeriodsController
    public async Task<ActionResult> Index()
    {
        var periods = await periodService.FindAll();
        return View(periods);
    }

    // GET: PeriodsController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: PeriodsController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: PeriodsController/Create
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

    // GET: PeriodsController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: PeriodsController/Edit/5
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

    // GET: PeriodsController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: PeriodsController/Delete/5
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