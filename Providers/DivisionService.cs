using DHR.Helper;
using DHR.Models;
using DHR.ViewModels.Divison;
using Microsoft.EntityFrameworkCore;

namespace DHR.Providers;

public class DivisionService(AppDbContext context)
{
    private const string StoreProcedure = "sp_Division";
    
    //FindAll
    public async Task<IEnumerable<DivisionModel>> FindAll()
    {
        var results = await context.Divisions
            .Include(d => d.SubDepartment)
            .Where(d => d.IsDeleted == false)
            .ToListAsync();
        return results;
    }

    //FindById
    public async Task<DivisionModel> FindById(int id)
    {
        var results = await context.Divisions
            .Where(x => x.DivisionId == id)
            .Include(d => d.SubDepartment)
            .FirstOrDefaultAsync();
        return results ?? new DivisionModel();
    }
    
    //Insert
    public async Task<int> Insert(CreateDivisionViewModel model, string userId, DateTime time)
    {
        var check = await context.Divisions.
            FirstOrDefaultAsync(x => x.DivisionCode == model.DivisionCode 
                                     && x.DivisionName == model.DivisionName
                                     && x.SubDepartmentId == model.SubDepartmentId);
        if (check != null)
        {
            return 3;
        }
        var results = await context.Database
            .ExecuteSqlAsync($@"
                    EXEC {StoreProcedure} 
                    @Action = 'Insert',
                    @DivisionCode = {model.DivisionCode},
                    @DivisionName = {model.DivisionName},
                    @SubDepartmentId = {model.SubDepartmentId},
                    @CreatedBy = {userId},
                    @CreatedAt = {time},
                    @UpdatedAt = {time},
                    @UpdatedBy = {userId}
           ");
        return results;
    }
    
    //Update
    public async Task<int> Update(EditDivisionViewModel model, string userId, DateTime time)
    {
        var results = await context.Database
            .ExecuteSqlAsync($@"
                    EXEC {StoreProcedure} 
                    @Action = 'Update',
                    @DivisionId = {model.DivisionId},
                    @DivisionCode = {model.DivisionCode},
                    @DivisionName = {model.DivisionName},
                    @SubDepartmentId = {model.SubDepartmentId},
                    @UpdatedBy = {userId},
                    @UpdatedAt = {time}
           ");
        return results;
    }
}