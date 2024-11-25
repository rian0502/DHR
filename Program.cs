using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DAHAR.Helper;
using DAHAR.Models;
using DAHAR.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<WorkAreaService>();
builder.Services.AddScoped<EducationService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<JobTitleService>();
builder.Services.AddScoped<PeriodService>();
builder.Services.AddScoped<UnitService>();
builder.Services.AddScoped<SubUnitService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<MongoDBContext>();

builder.Services.AddScoped<AttendanceService>(serviceProvider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new AttendanceService(connectionString);
});

builder.Services.AddIdentity<Users, IdentityRole>(options => {
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
}).AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();


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

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        await DatabaseSeeder.SeedData(services);
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error seeding data: {ex.Message}");
//    }
//}

app.Run();
