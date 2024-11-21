using DAHAR.Helper;
using DAHAR.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DAHAR.Controllers;

public class AppLogController(MongoDBContext mongoDbContext) : Controller
{
    public async Task<IActionResult> Index()
    {

        var logs = await mongoDbContext.AppLogs.Find(FilterDefinition<AppLogModel>.Empty)
            .Sort(Builders<AppLogModel>.Sort.Descending("CreatedAt"))
            .Limit(30)
            .ToListAsync();
        return View(logs);
    }
}