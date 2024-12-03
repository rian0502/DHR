using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.ViewModels.Education;
using Microsoft.EntityFrameworkCore;

namespace DAHAR.Providers;

public class EducationService(AppDbContext context)
{
    private const string StoreProcedure = "sp_Education";

    //FindAll
    public async Task<IEnumerable<EducationModel>> FindAll()
    {
        var results = await context.Educations
            .FromSql($"EXEC {StoreProcedure} @Action = 'FindAll'")
            .ToListAsync();
        return results;
    }
    //FindById
    public async Task<EducationModel> FindById(int id)
    {
        var results = await context.Educations
            .FromSqlInterpolated($"EXEC {StoreProcedure} @Action = 'FindById', @Id = {id}")
            .ToListAsync(); 
        return results.FirstOrDefault();
    }
    
    public async Task<int> Insert(CreateEducationViewModel model, string user, DateTime time)
    {
        var affectedRows = await context.Database
            .ExecuteSqlAsync($"EXEC {StoreProcedure} @Action = 'Insert', @Name = {model.EducationName}, @CreatedBy = {user}, @CreatedAt = {time}, @UpdatedAt = {time}, @UpdatedBy = {user}");
        return affectedRows; // Return affected rows count
    }

    //Update
    public async Task<int> Update(EditEducationViewModel model, string user, DateTime time)
    {
        var affectedRows = await context.Database
            .ExecuteSqlAsync($"EXEC {StoreProcedure} @Action = 'Update', @Id = {model.EducationId}, @Name = {model.EducationName}, @UpdatedBy = {user}, @UpdatedAt = {time}");
        return affectedRows;
    }
}