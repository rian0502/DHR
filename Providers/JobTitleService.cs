using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.ViewModels.JobTitle;
using Microsoft.EntityFrameworkCore;

namespace DAHAR.Providers
{
    public class JobTitleService(AppDBContext context)
    {
        private readonly AppDBContext _context = context;
        private readonly string _storeProcedure = "sp_JobTitle";

        //FindAll
        public async Task<IEnumerable<JobTitleModel>> FindAll()
        {
            var results = await _context.JobTitles.FromSql($"EXEC {_storeProcedure} @Action = 'FindAll'").ToListAsync();
            return results;
        }

        //FindById
        public async Task<JobTitleModel> FindById(int id)
        {
            var result = await _context.JobTitles.FromSql($"EXEC {_storeProcedure} @Action = 'FindById', @Id = {id}")
                .ToListAsync();
            return result.FirstOrDefault();
        }

        //Insert
        public async Task<int> Insert(CreateJobTitleViewModel jobTitle, string userId, DateTime time)
        {
            //check if code already exist
            var check = await _context.JobTitles.FirstOrDefaultAsync(x => x.JobTitleCode == jobTitle.JobTitleCode);
            if (check != null)
            {
                return 3;
            }

            var result = await _context.Database.ExecuteSqlAsync($@"
                    EXEC {_storeProcedure} 
                    @Action = 'Insert',
                    @Name = {jobTitle.JobTitleName}, 
                    @Code = {jobTitle.JobTitleCode}, 
                    @Description = {jobTitle.JobTitleDescription},
                    @CreatedBy = {userId},
                    @CreatedAt = {time},
                    @UpdatedBy = {userId},
                    @UpdatedAt = {time}
                ");
            return result;
        }

        //Update
        public async Task<int> Update(EditJobTitleViewModel jobTitle, string userId, DateTime time)
        {
            var result = await _context.Database.ExecuteSqlAsync($@"
                    EXEC {_storeProcedure} 
                    @Action = 'Update', 
                    @Id = {jobTitle.JobTitleID}, 
                    @Name = {jobTitle.JobTitleName}, 
                    @Code = {jobTitle.JobTitleCode}, 
                    @Description = {jobTitle.JobTitleDescription},
                    @UpdatedBy = {userId},
                    @UpdatedAt = {time}
            ");
            return result;
        }
    }
}