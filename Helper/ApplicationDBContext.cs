using Microsoft.EntityFrameworkCore;
using Presensi360.Models;

namespace Presensi360.Helper
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<CompanyModel> Company { get; set; }
    }
}
