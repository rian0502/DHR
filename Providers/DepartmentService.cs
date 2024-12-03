using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.ViewModels.Department;
using Microsoft.EntityFrameworkCore;

namespace DAHAR.Providers;

public class DepartmentService(AppDbContext context)
{
    private const string StoreProcedure = "sp_Department";


    //FindAll
    public async Task<IEnumerable<DepartmentModel>> FindAll()
    {
        var results = await context.Departments
            .Include(d => d.Company)
            .ToListAsync();
        return results;
    }

    //FindById
    public async Task<DepartmentModel> FindById(int id)
    {
        var results = await context.Departments
            .Where(x => x.DepartmentId == id)
            .Include(d => d.Company)
            .FirstOrDefaultAsync();
        return results ?? new DepartmentModel();
    }
    //Insert
    public async Task<int> Insert(CreateDepartmentViewModel model, string userId, DateTime time)
    {
        var check = await context.Departments.
            FirstOrDefaultAsync(x => x.DepartmentCode == model.DepartmentCode 
                                     && x.CompanyId == model.CompanyId
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
                    @CompanyID = {model.CompanyId},
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
                    @CompanyID = {model.CompanyId},
                    @UpdatedAt = {time},
                    @UpdatedBy = {userId}
           ");
        return results;
    }
}