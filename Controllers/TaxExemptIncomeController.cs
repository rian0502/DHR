using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.Providers;
using DAHAR.ViewModels.TaxExemptIncome;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DAHAR.Controllers;

[Authorize(Roles = "Admin")]
public class TaxExemptIncomeController(
    TaxExemptService taxExemptService,
    UserManager<Users> userManager,
    MongoDbContext mongoDbContext) : Controller
{
    // GET: TaxExemptIncomeController
    public async Task<ActionResult> Index()
    {
        var taxes = await taxExemptService.FindAll();
        return View(taxes);
    }

    // GET: TaxExemptIncomeController/Create
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<ActionResult> Create(CreateTaxExemptIncomeViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            var time = DateTime.UtcNow;
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var insert = await taxExemptService.Insert(model, user.Id, time);
            switch (insert)
            {
                case 0:
                    TempData["Errors"] = "Insert failed";
                    break;
                case 3:
                    TempData["Errors"] = "Tax Exempt Income Code or Name already exists";
                    break;
                default:
                    var logs = new AppLogModel
                    {
                        CreatedAt = time,
                        CreatedBy = $"{user.Id} - {user.FullName}",
                        Params = JsonConvert.SerializeObject(model),
                        Source = JsonConvert.SerializeObject(new
                        {
                            Controller = "TaxExemptIncomeController",
                            Action = "Create",
                            Database = "TaxExemptIncomes",
                        }),
                    };
                    await mongoDbContext.AppLogs.InsertOneAsync(logs);
                    TempData["Success"] = "Tax Exempt Income has been added";
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

    // GET: TaxExemptIncomeController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        var tax = await taxExemptService.FindById(id);

        return View(new EditTaxExemptIncomeViewModel
        {
            TaxExemptIncomeId = tax.TaxExemptIncomeId,
            TaxExemptIncomeCode = tax.TaxExemptIncomeCode ?? "",
            TaxExemptIncomeName = tax.TaxExemptIncomeName ?? ""
        });
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Edit(int id, EditTaxExemptIncomeViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var oldData = await taxExemptService.FindById(id);
            var user = await userManager.GetUserAsync(User);
            var time = DateTime.UtcNow;
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var update = await taxExemptService.Update(model, user.Id, time);
            if (update == 0)
            {
                TempData["Errors"] = "Update failed";
            }
            else
            {
                var logs = new AppLogModel
                {
                    CreatedAt = time,
                    CreatedBy = $"{user.Id} - {user.FullName}",
                    Params = JsonConvert.SerializeObject(new
                    {
                        oldData = JsonConvert.SerializeObject(new
                        {
                            oldData.TaxExemptIncomeId,
                            oldData.TaxExemptIncomeCode,
                            oldData.TaxExemptIncomeName,
                            oldData.CreatedAt,
                            oldData.CreatedBy,
                            oldData.UpdatedAt,
                            oldData.UpdatedBy
                        }),
                        newData = JsonConvert.SerializeObject(model)
                    }),
                    Source = JsonConvert.SerializeObject(new
                    {
                        Controller = "TaxExemptIncomeController",
                        Action = "Edit",
                        Database = "TaxExemptIncomes",
                    }),
                };
                await mongoDbContext.AppLogs.InsertOneAsync(logs);
                TempData["Success"] = "Tax Exempt Income has been updated";
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