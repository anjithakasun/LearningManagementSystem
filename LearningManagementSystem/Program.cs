using ComplaignManagementSystem.Data.Context;
using ComplaignManagementSystem.Presentation.Filters;
using ComplaintManagementSystem.Business.Authentication;
using ComplaintManagementSystem.Business.ConncetionHandler;
using LearningManagementSystem.Bussiness.TrainingHandler;
using LearningManagementSystem.Bussiness.UserHandler;
//using LearningManagementSystem.Data.LMSModels;
using Microsoft.EntityFrameworkCore;
using log4net;
using Microsoft.AspNetCore.Http;
using LearningManagementSystem.Data.LMSModels;
using System.Data.SqlClient;
using LearningManagementSystem.Bussiness.LearningManagementHandler;
using LearningManagementSystem.Bussiness.CourseHandler;

var builder = WebApplication.CreateBuilder(args);

string rawConnectionString = builder.Configuration.GetConnectionString("Connection");

// Force-disabling SSL & trusting server certificate
var sqlBuilder = new SqlConnectionStringBuilder(rawConnectionString)
{
    Encrypt = false,
    TrustServerCertificate = true
};

// Register EF Core with the modified connection string
builder.Services.AddDbContext<LearningManagementContext>(options =>
    options.UseSqlServer(sqlBuilder.ConnectionString));


builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<SessionCheckAttribute>();
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<LearningManagementContext>();

builder.Services.AddScoped<DapperContext>();

builder.Services.AddScoped<ADAuthentication>();

builder.Services.AddScoped<_ConnectionService>();

builder.Services.AddScoped<ILearningService, LearningService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ITrainingService, TrainingService>();

builder.Services.AddScoped<ICourseService, CourseService>();

builder.Logging.ClearProviders(); // Optional: clear default providers
builder.Logging.AddLog4Net("log4net.config");

builder.Logging.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    //options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
    //        ? CookieSecurePolicy.None
    //        : CookieSecurePolicy.Always;
    //options.Cookie.SameSite = SameSiteMode.Strict;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");
app.UseWebSockets(); 
app.Run();
