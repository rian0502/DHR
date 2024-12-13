using DHR.Helper;
using DHR.Models;
using DHR.ViewModels.SubDepartment;
using Microsoft.EntityFrameworkCore;

namespace DHR.Providers;

public class SubDepartmentService(AppDbContext context)
{
    private const string StoreProcedure = "sp_SubDepartment";

    //FindAll
    public async Task<IEnumerable<SubDepartmentModel>> FindAll()
    {
        var subDepartments = await context.SubDepartments
            .Include(s => s.Department)
            .ToListAsync();
        return subDepartments;
    }
    
    //FindById
    public async Task<SubDepartmentModel> FindById(int id)
    {
        var subDepartment = await context.SubDepartments
            .Include(s => s.Department)
            .FirstOrDefaultAsync(s => s.SubDepartmentId == id);
        return subDepartment ?? new SubDepartmentModel();
    }
    
    //Insert
    public async Task<int> Insert(CreateSubDepartmentViewModel model, string userId, DateTime time)
    {
        var check = await context.SubDepartments.
            FirstOrDefaultAsync(x => x.SubDepartmentCode == model.SubDepartmentCode 
                                     && x.SubDepartmentName == model.SubDepartmentName
                                     && x.DepartmentId == model.DepartmentId);
        if (check != null)
        {
            return 3;
        }
        var results = await context.Database
            .ExecuteSqlAsync($@"
                    EXEC {StoreProcedure} 
                    @Action = 'Insert',
                    @SubDepartmentCode = {model.SubDepartmentCode},
                    @SubDepartmentName = {model.SubDepartmentName},
                    @DepartmentID = {model.DepartmentId},
                    @CreatedBy = {userId},
                    @CreatedAt = {time},
                    @UpdatedAt = {time},
                    @UpdatedBy = {userId}
           ");
        return results;
    }
    public async Task<int> Update(EditSubDepartmentViewModel model, string userId, DateTime time)
    {
        var results = await context.Database
            .ExecuteSqlAsync($@"
                    EXEC {StoreProcedure} 
                    @Action = 'Update',
                    @SubDepartmentID = {model.SubDepartmentId},
                    @SubDepartmentCode = {model.SubDepartmentCode},
                    @SubDepartmentName = {model.SubDepartmentName},
                    @DepartmentID = {model.DepartmentId},
                    @UpdatedAt = {time},
                    @UpdatedBy = {userId}
           ");
        return results;
    }
    
}