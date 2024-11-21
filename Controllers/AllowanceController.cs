using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class AllowanceController : Controller
{
    // GET: AllowanceController
    public ActionResult Index()
    {
        return View();
    }

    // GET: AllowanceController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: AllowanceController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: AllowanceController/Create
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

    // GET: AllowanceController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: AllowanceController/Edit/5
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

    // GET: AllowanceController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: AllowanceController/Delete/5
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