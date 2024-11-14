using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Presensi360.Models;

namespace Presensi360.Helper
{
    public class AppDBContext : IdentityDbContext<Users>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<LocationModel> Locations { get; set; }
        public DbSet<CompanyModel> Companies { get; set; }
        public DbSet<JobTitleModel> JobTitles { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<SubDepartmentModel> SubDepartments { get; set; }
        public DbSet<EmployeeModel> Employee { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<EmployeeModel>().HasOne(e => e.User).WithOne().HasForeignKey<EmployeeModel>(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CompanyModel>().HasOne(c => c.Location).WithMany(l => l.Companies).HasForeignKey(c => c.LocationID).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
