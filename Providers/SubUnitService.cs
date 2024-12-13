using DHR.Helper;
using DHR.Models;
using DHR.ViewModels.SubUnit;
using Microsoft.EntityFrameworkCore;

namespace DHR.Providers;

public class SubUnitService(AppDbContext context)
{
    private const string StoreProcedure = "sp_SubUnit";

    //FindAll
    public async Task<IEnumerable<SubUnitModel>> FindAll()
    {
        var subUnits = await context.SubUnits
            .Include(s => s.Unit)
            .Include(s => s.Location)
            .ToListAsync();
        return subUnits;
    }

    //FindById
    public async Task<SubUnitModel> FindById(int id)
    {
        var subUnit = await context.SubUnits
            .Include(s => s.Unit)
            .Include(s => s.Location)
            .FirstOrDefaultAsync(s => s.SubUnitId == id);
        return subUnit;
    }

    //Insert
    public async Task<int> Insert(CreateSubUnitViewModel subUnit, string userId, DateTime time)
    {
        var check = await context.Units.FirstOrDefaultAsync(x =>
            x.UnitCode == subUnit.SubUnitCode || x.UnitName == subUnit.SubUnitName);
        if (check != null)
        {
            return 3;
        }

        var result = await context.Database.ExecuteSqlAsync($@"
                      EXEC {StoreProcedure}
                      @Action = 'Insert', 
                      @Code = {subUnit.SubUnitCode},
                      @Name = {subUnit.SubUnitName}, 
                      @Address = {subUnit.SubUnitAddress}, 
                      @UnitID = {subUnit.UnitId}, 
                      @LocationID = {subUnit.LocationId}, 
                      @CreatedBy = {userId},
                      @CreatedAt = {time},
                      @UpdatedBy = {userId},
                      @UpdatedAt = {time}
            ");
        return result;
    }
    
    public async Task<int> Update(EditSubUnitViewModel subUnit, string userId, DateTime time)
    {
        var result = await context.Database.ExecuteSqlAsync($@"
                      EXEC {StoreProcedure}
                      @Action = 'Update',
                      @Id = {subUnit.SubUnitId},
                      @Code = {subUnit.SubUnitCode},
                      @Name = {subUnit.SubUnitName}, 
                      @Address = {subUnit.SubUnitAddress}, 
                      @UnitID = {subUnit.UnitId}, 
                      @LocationID = {subUnit.LocationId}, 
                      @UpdatedBy = {userId},
                      @UpdatedAt = {time}
            ");
        return result;
    }
}