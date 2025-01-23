using DHR.Helper;
using DHR.Models;
using DHR.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<MongoDbContext>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(connectionString);

/*
 * Register Services
 */
builder.Services.AddScoped<AttendanceService>();
builder.Services.AddScoped<YearPeriodService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<MedicalClaimService>();
builder.Services.AddScoped<LeaveRequestService>();
builder.Services.AddScoped<PermissionRequestService>();
builder.Services.AddScoped<WorkEntryService>();

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
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);   
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
app.Use(async (context, next) =>
{
    string cookie = string.Empty;
    if (context.Request.Cookies.TryGetValue("lang", out cookie))
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie);
        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie);
    }
    else
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
    }
    await next.Invoke();
});
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Dashboard}/{id?}");

app.Run();
