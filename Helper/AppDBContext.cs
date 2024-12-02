using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using DAHAR.Models;

namespace DAHAR.Helper
{
    public class AppDBContext : IdentityDbContext<Users>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<FormApplicationRequestModel> FormApplication { get; set; }
        public DbSet<AttendanceStatusModel> AttendanceStatus { get; set; }
        public DbSet<PeriodModel> Periods { get; set; }
        public DbSet<LocationModel> Locations { get; set; }
        public DbSet<CompanyModel> Companies { get; set; }
        public DbSet<JobTitleModel> JobTitles { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<SubDepartmentModel> SubDepartments { get; set; }
        public DbSet<DivisionModel> Divisions { get; set; }
        public DbSet<ReligionModel> Religions { get; set; }
        public DbSet<EducationModel> Educations { get; set; }
        public DbSet<TaxExemptIncomeModel> TaxExemptIncomes { get; set; }
        public DbSet<UnitModel> Units { get; set; }
        public DbSet<SubUnitModel> SubUnits { get; set; }
        public DbSet<EmployeeModel> Employee { get; set; }
        public DbSet<EmployeeDependentModel> EmployeeDependents { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<LocationModel>().HasMany(l => l.Companies).WithOne(c => c.Location).HasForeignKey(c => c.LocationID);
            builder.Entity<CompanyModel>().HasMany(d => d.Departments).WithOne(d => d.Company).HasForeignKey(d => d.CompanyId);
            builder.Entity<DepartmentModel>().HasMany(sd => sd.SubDepartments).WithOne(sd => sd.Department).HasForeignKey(sd => sd.DepartmentID);
            builder.Entity<UnitModel>().HasMany(d => d.SubUnits).WithOne(d => d.Unit).HasForeignKey(d => d.UnitID);
            builder.Entity<EmployeeModel>().HasOne(e => e.SubUnit).WithMany(su => su.Employees).HasForeignKey(e => e.SubUnitId);
            builder.Entity<EmployeeModel>().HasOne(e => e.Users).WithOne().HasForeignKey<EmployeeModel>(e => e.UserId);
            builder.Entity<EmployeeModel>().HasOne(e => e.Division).WithMany(d => d.Employees).HasForeignKey(e => e.DivisionID);
            builder.Entity<EmployeeModel>().HasOne(e => e.JobTitle).WithMany(jt => jt.Employees).HasForeignKey(e => e.JobTitleID);
            builder.Entity<EmployeeModel>().HasOne(e => e.Religion).WithMany(r => r.Employees).HasForeignKey(e => e.ReligionId);
            builder.Entity<EmployeeModel>().HasOne(e => e.Education).WithMany(ed => ed.Employees).HasForeignKey(e => e.EducationId);
            builder.Entity<EmployeeModel>().HasOne(e => e.TaxExemptIncome).WithMany(tei => tei.Employees).HasForeignKey(e => e.TaxExemptIncomeId);
            builder.Entity<EmployeeModel>().HasMany(e => e.EmployeeDependents).WithOne(ed => ed.Employee).HasForeignKey(ed => ed.EmployeeId);
        }
    }
}
