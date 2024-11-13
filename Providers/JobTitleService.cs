using Microsoft.EntityFrameworkCore;
using Presensi360.Helper;
using Presensi360.Models;
using Presensi360.ViewModels;

namespace Presensi360.Providers
{
    public class JobTitleService(AppDBContext context)
    {
        private readonly AppDBContext _context = context;
        private readonly string _storeProcedure = "sp_JobTitle";

        //FindAll
        public async Task<IEnumerable<JobTitle>> FindAll()
        {
            var results = await _context.JobTitles.FromSql($"EXEC {_storeProcedure} @Action = 'FindAll'").ToListAsync();
            return results;
        }

        //FindById
        public async Task<JobTitle> FindById(int id)
        {
            var result = await _context.JobTitles.FromSql($"EXEC {_storeProcedure} @Action = 'FindById', @Id = {id}").ToListAsync();
            return result.FirstOrDefault();
        }

        //Insert
        public async Task<int> Insert(CreateJobTitleViewModel jobTitle)
        {
            //check if code already exist
            var check = await _context.JobTitles.FirstOrDefaultAsync(x => x.JobTitleCode == jobTitle.JobTitleCode);
            if (check != null)
            {
                return 3;
            }
            var result = await _context.Database.ExecuteSqlAsync($"EXEC {_storeProcedure} @Action = 'Insert', @Name = {jobTitle.JobTitleName}, @Code = {jobTitle.JobTitleCode}, @Description = {jobTitle.JobTitleDescription}");
            return result;
        }

        //Update
        public async Task<int> Update(EditJobTitleViewModel jobTitle)
        {
            var result = await _context.Database.ExecuteSqlAsync($"EXEC {_storeProcedure} @Action = 'Update', @Id = {jobTitle.JobTitleID}, @Name = {jobTitle.JobTitleName}, @Code = {jobTitle.JobTitleCode}, @Description = {jobTitle.JobTitleDescription}");
            return result;
        }
    }
}
