using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.ViewModels.TaxExemptIncome;
using Microsoft.EntityFrameworkCore;

namespace DAHAR.Providers;

public class TaxExemptService(AppDbContext context)
{
    private const string StoreProcedure = "sp_TaxExemptIncome";

    //FindAll
    public async Task<IEnumerable<TaxExemptIncomeModel>> FindAll()
    {
        var results = await context.TaxExemptIncomes
            .FromSql($"EXEC {StoreProcedure} @Action = 'FindAll'")
            .ToListAsync();
        return results;
    }

    //FindById
    public async Task<TaxExemptIncomeModel> FindById(int id)
    {
        var results = await context.TaxExemptIncomes
            .FromSqlInterpolated($"EXEC {StoreProcedure} @Action = 'FindById', @TaxExemptIncomeId = {id}")
            .ToListAsync();
        return results.FirstOrDefault() ?? new TaxExemptIncomeModel();
    }

    //insert
    public async Task<int> Insert(CreateTaxExemptIncomeViewModel model, string userId, DateTime time)
    {
        var check = await context.TaxExemptIncomes.FirstOrDefaultAsync(x =>
            x.TaxExemptIncomeCode == model.TaxExemptIncomeCode ||
            x.TaxExemptIncomeName == model.TaxExemptIncomeName);
        if (check != null)
        {
            return 3;
        }

        var result = await context.Database
            .ExecuteSqlAsync($@"
                    EXEC {StoreProcedure} 
                    @Action = 'Insert', 
                    @TaxExemptIncomeCode = {model.TaxExemptIncomeCode}, 
                    @TaxExemptIncomeName = {model.TaxExemptIncomeName}, 
                    @CreatedBy = {userId}, 
                    @CreatedAt = {time}, 
                    @UpdatedAt = {time}, 
                    @UpdatedBy = {userId}
            ");
        return result;
    }

    //Update
    public async Task<int> Update(EditTaxExemptIncomeViewModel model, string user, DateTime time)
    {
        var result = await context.Database
            .ExecuteSqlAsync($@"
                    EXEC {StoreProcedure}
                    @Action = 'Update',
                    @TaxExemptIncomeId = {model.TaxExemptIncomeId},
                    @TaxExemptIncomeCode = {model.TaxExemptIncomeCode},
                    @TaxExemptIncomeName = {model.TaxExemptIncomeName},
                    @UpdatedBy = {user},
                    @UpdatedAt = {time}
            ");
        return result;
    }
}