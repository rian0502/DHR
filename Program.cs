using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DHR.Helper;
using DHR.Models;
using DHR.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Daftarkan DbContext dengan koneksi ke SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Daftarkan MongoDB Context
builder.Services.AddScoped<MongoDbContext>();

// Daftarkan string koneksi sekali
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(connectionString);

// Daftarkan service yang memerlukan koneksi database
builder.Services.AddScoped<AttendanceService>();
builder.Services.AddScoped<YearPeriodService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<MedicalClaimService>();

// Daftarkan service lainnya
builder.Services.AddScoped<UnitService>();
builder.Services.AddScoped<PeriodService>();
builder.Services.AddScoped<SubUnitService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<WorkAreaService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<JobTitleService>();
builder.Services.AddScoped<DivisionService>();
builder.Services.AddScoped<EducationService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<TaxExemptService>();
builder.Services.AddScoped<SubDepartmentService>();

// Konfigurasi Identity
builder.Services.AddIdentity<Users, IdentityRole>(options => {
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Konfigurasi Antiforgery
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Dashboard}/{id?}");

// Uncomment kode di bawah ini untuk seeding data
// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     try
//     {
//         await DatabaseSeeder.SeedData(services);
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Error seeding data: {ex.Message}");
//     }
// }

app.Run();
