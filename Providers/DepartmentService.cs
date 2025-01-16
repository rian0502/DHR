using DHR.Helper;
using DHR.Models;
using DHR.ViewModels.Department;
using Microsoft.EntityFrameworkCore;

namespace DHR.Providers;

public class DepartmentService(AppDbContext context)
{
    private const string StoreProcedure = "sp_Department";


    //FindAll
    public async Task<IEnumerable<DepartmentModel>> FindAll()
    {
        var results = await context.Departments
            .Where(d => d.IsDeleted == false)
            .ToListAsync();
        return results;
    }

    //FindById
    public async Task<DepartmentModel> FindById(int id)
    {
        var results = await context.Departments
            .Where(x => x.DepartmentId == id)
            .FirstOrDefaultAsync();
        return results ?? new DepartmentModel();
    }
    //Insert
    public async Task<int> Insert(CreateDepartmentViewModel model, string userId, DateTime time)
    {
        var check = await context.Departments.
            FirstOrDefaultAsync(x => x.DepartmentCode == model.DepartmentCode
                                     && x.DepartmentName == model.DepartmentName);
        if (check != null)
        {
            return 3;
        }
        var results = await context.Database
            .ExecuteSqlAsync($@"
                    EXEC {StoreProcedure} 
                    @Action = 'Insert',
                    @DepartmentCode = {model.DepartmentCode},
                    @DepartmentName = {model.DepartmentName},
                    @CreatedBy = {userId},
                    @CreatedAt = {time},
                    @UpdatedAt = {time},
                    @UpdatedBy = {userId}
           ");
        return results;
    }
    
    //Update
    public async Task<int> Update(EditDepartmentViewModel model, string userId, DateTime time)
    {
        var results = await context.Database
            .ExecuteSqlAsync($@"
                    EXEC {StoreProcedure} 
                    @Action = 'Update',
                    @DepartmentID = {model.DepartmentId},
                    @DepartmentCode = {model.DepartmentCode},
                    @DepartmentName = {model.DepartmentName},
                    @UpdatedAt = {time},
                    @UpdatedBy = {userId}
           ");
        return results;
    }
}