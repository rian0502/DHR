using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DHR.Models;

namespace DHR.Helper
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<Users>(options)
    {
        public DbSet<FormApplicationRequestModel> FormApplication { get; set; }
        public DbSet<AttendanceStatusModel> AttendanceStatus { get; set; }
        public DbSet<PeriodModel> Periods { get; set; }
        public DbSet<BenefitModel> Benefits { get; set; }
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
        public DbSet<EmployeeBenefit> EmployeeBenefits { get; set; }
        
        public DbSet<EmployeeLeaveRequestModel> EmployeeLeaveRequest { get; set; }
        public DbSet<EmployeePermissionRequest> EmployeePermissionRequest { get; set; }
        public DbSet<EmployeeMedicalClaim> EmployeeMedicalClaims { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PeriodModel>().HasMany(pm => pm.EmployeeMedicalClaims).WithOne(emc => emc.Period).HasForeignKey(emc => emc.PeriodId);
            builder.Entity<BenefitModel>().HasMany(eb => eb.EmployeeBenefits).WithOne(eb => eb.Benefit).HasForeignKey(eb => eb.BenefitId);
            builder.Entity<LocationModel>().HasMany(l => l.Companies).WithOne(c => c.Location).HasForeignKey(c => c.LocationId);
            builder.Entity<CompanyModel>().HasMany(c => c.Employees).WithOne(e => e.Company).HasForeignKey(e => e.CompanyId);
            builder.Entity<DepartmentModel>().HasMany(sd => sd.SubDepartments).WithOne(sd => sd.Department).HasForeignKey(sd => sd.DepartmentId);
            builder.Entity<UnitModel>().HasMany(d => d.SubUnits).WithOne(d => d.Unit).HasForeignKey(d => d.UnitId);
            builder.Entity<EmployeeModel>().HasOne(e => e.SubUnit).WithMany(su => su.Employees).HasForeignKey(e => e.SubUnitId);
            builder.Entity<EmployeeModel>().HasOne(e => e.Users).WithOne().HasForeignKey<EmployeeModel>(e => e.UserId);
            builder.Entity<EmployeeModel>().HasOne(e => e.Division).WithMany(d => d.Employees).HasForeignKey(e => e.DivisionId);
            builder.Entity<EmployeeModel>().HasOne(e => e.JobTitle).WithMany(jt => jt.Employees).HasForeignKey(e => e.JobTitleId);
            builder.Entity<EmployeeModel>().HasOne(e => e.Religion).WithMany(r => r.Employees).HasForeignKey(e => e.ReligionId);
            builder.Entity<EmployeeModel>().HasOne(e => e.Education).WithMany(ed => ed.Employees).HasForeignKey(e => e.EducationId);
            builder.Entity<EmployeeModel>().HasOne(e => e.TaxExemptIncome).WithMany(tei => tei.Employees).HasForeignKey(e => e.TaxExemptIncomeId);
            builder.Entity<EmployeeModel>().HasMany(e => e.EmployeeDependents).WithOne(ed => ed.Employee).HasForeignKey(ed => ed.EmployeeId);
            builder.Entity<EmployeeModel>().HasMany(e => e.Benefits).WithOne(eb => eb.Employee).HasForeignKey(eb => eb.EmployeeId);
            builder.Entity<EmployeeModel>().HasMany(e => e.MedicalClaims).WithOne(mc => mc.Employee).HasForeignKey(mc => mc.EmployeeId);
            builder.Entity<EmployeeModel>().HasMany(e => e.EmployeePermissions).WithOne(ep => ep.Employee).HasForeignKey(ep => ep.EmployeeId);
            builder.Entity<EmployeeModel>().HasMany(e => e.EmployeeLeaveRequestModels).WithOne(elr => elr.Employee).HasForeignKey(elr => elr.EmployeeId);
            
        }
    }
}
