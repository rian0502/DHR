using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.ViewModels;
using DAHAR.ViewModels.Company;
using Microsoft.EntityFrameworkCore;

namespace DAHAR.Providers
{
    public class CompanyService(AppDBContext context)
    {
        private readonly AppDBContext _context = context;
        private readonly string _storeProcedure = "sp_Company";

        // FindAll
        public async Task<IEnumerable<CompanyModel>> FindAll()
        {
            var results = await _context.Companies
                .Include(c => c.Location)
                .ToListAsync();
            return results;
        }

        // FindById
        public async Task<CompanyModel> FindById(int id)
        {
            var results = await _context.Companies
                .FromSqlInterpolated($"EXEC {_storeProcedure} @Action = 'FindById', @Id = {id}")
                .ToListAsync();
            return results.FirstOrDefault();
        }

        // Insert
        public async Task<int> Insert(CreateCompanyViewModel company, string userId, DateTime time)
        {
            //check if code already exist
            var check = await _context.Companies
                .FirstOrDefaultAsync(x => x.CompanyCode == company.CompanyCode);
            if (check != null)
            {
                return 3;
            }

            var result = await _context.Database.ExecuteSqlAsync($@"
                    EXEC {_storeProcedure} 
                    @Action = 'Insert',
                    @Code = {company.CompanyCode},
                    @Name = {company.CompanyName},
                    @LocationID = {company.LocationID},
                    @CreatedBy = {userId},
                    @CreatedAt = {time},
                    @UpdatedBy = {userId},
                    @UpdatedAt = {time}
            ");
            return result;
        }

        // Update
        public async Task<int> Update(EditCompanyViewModel company, string userId, DateTime time)
        {
            var result = await _context.Database.ExecuteSqlAsync($@"
                    EXEC {_storeProcedure} 
                    @Action = 'Update', 
                    @Id = {company.CompanyID}, 
                    @Code = {company.CompanyCode}, 
                    @Name = {company.CompanyName}, 
                    @LocationID = {company.LocationID},
                    @UpdatedBy = {userId},
                    @UpdatedAt = {time}
            ");
            return result;
        }
    }
}