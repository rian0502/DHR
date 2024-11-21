using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.ViewModels;

namespace DAHAR.Providers;

public class WorkAreaService(AppDBContext context)
{
    private readonly AppDBContext _context = context;
    private readonly string _storeProcedure = "sp_Location";

    //FindAll
    public async Task<IEnumerable<LocationModel>> FindAll()
    {
        var results = await _context.Locations
            .FromSql($"EXEC {_storeProcedure} @Action = 'FindAll'")
            .ToListAsync();
        return results;
    }
    //FindById
    public async Task<LocationModel> FindById(int id)
    {
        var results = await _context.Locations
            .FromSqlInterpolated($"EXEC {_storeProcedure} @Action = 'FindById', @Id = {id}")
            .ToListAsync();
        return results.FirstOrDefault();
    }
    //Create
    public async Task<int> Create(CreateWorkAreaViewModel model, string userId)
    {
        var currentTime = DateTime.UtcNow;
        var result = await _context.Database.ExecuteSqlAsync($@"
            EXEC {_storeProcedure} 
                @Action = 'Insert',
                @Code = {model.LocationCode},
                @Name = {model.LocationName},
                @CreatedBy = {userId},
                @CreatedAt = {currentTime},
                @UpdatedBy = {userId},
                @UpdatedAt = {currentTime}
        ");

        return result;
    }
    //Update
    public async Task<int> Update(EditWorkAreaViewModel model)
    {
        var result = await _context.Database.ExecuteSqlAsync($"EXEC {_storeProcedure} @Action = 'Update', @Id = {model.LocationID}, @Code = {model.LocationCode}, @Name = {model.LocationName}");
        return result;
    }

}
