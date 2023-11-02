using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddControllersWithViews();
services.AddHttpContextAccessor();
services.AddSingleton<AppDbContext>(_ => 
    new AppDbContext(builder.Configuration.GetConnectionString("Default")));

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IAuthenticationService, AuthenticationService>();
services.AddScoped<IIndividualRepository, IndividualRepository>();
services.AddScoped<ITransactionRepository, TransactionRepository>();


// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-6.0
// https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims?view=aspnetcore-7.0
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

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

app.UseAuthentication();
app.UseAuthorization();

// app.MapRazorPages();
app.MapDefaultControllerRoute();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
