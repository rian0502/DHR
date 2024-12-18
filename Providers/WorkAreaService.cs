using Microsoft.EntityFrameworkCore;
using DHR.Helper;
using DHR.Models;
using DHR.ViewModels.WorkArea;

namespace DHR.Providers;

public class WorkAreaService(AppDbContext context)
{
    private const string StoreProcedure = "sp_Location";

    //FindAll
    public async Task<IEnumerable<LocationModel>> FindAll()
    {
        var results = await context.Locations
            .FromSql($"EXEC {StoreProcedure} @Action = 'FindAll'")
            .ToListAsync();
        return results;
    }
    //FindById
    public async Task<LocationModel> FindById(int id)
    {
        var results = await context.Locations
            .FromSql($"EXEC {StoreProcedure} @Action = 'FindById', @Id = {id}")
            .ToListAsync();
        return results.FirstOrDefault() ?? new LocationModel();
    }
    //Create
    public async Task<int> Create(CreateWorkAreaViewModel model, string userId, DateTime time)
    {
        var result = await context.Database.ExecuteSqlAsync($@"
            EXEC {StoreProcedure} 
                @Action = 'Insert',
                @Code = {model.LocationCode},
                @Name = {model.LocationName},
                @CreatedBy = {userId},
                @CreatedAt = {time},
                @UpdatedBy = {userId},
                @UpdatedAt = {time}
        ");
        return result;
    }
    //Update
    public async Task<int> Update(EditWorkAreaViewModel model, string userId, DateTime time)
    {
        var result= await context.Database.ExecuteSqlAsync($@"
            EXEC {StoreProcedure} 
                @Action = 'Update',
                @Id = {model.LocationId},
                @Code = {model.LocationCode},
                @Name = {model.LocationName},
                @UpdatedBy = {userId},
                @UpdatedAt = {time}
        ");
        return result;
    }

}
