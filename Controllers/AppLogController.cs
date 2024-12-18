using DHR.Helper;
using DHR.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;

namespace DHR.Controllers;

public class AppLogController(MongoDbContext mongoDbContext) : Controller
{
    public async Task<IActionResult> Index()
    {
        var logs = await mongoDbContext.AppLogs
            .Find(FilterDefinition<AppLogModel>.Empty)
            .Sort(Builders<AppLogModel>.Sort.Descending(nameof(AppLogModel.CreatedAt)))
            .Limit(10)
            .ToListAsync();
        return View(logs);
    }
}