using System.Security.Claims;
using BankingSystem.Constants;
using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using BankingSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddControllersWithViews();
services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("Default");

services.AddSingleton<AppDbContext>(_ => 
    new AppDbContext(connectionString));

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IPasswordService, PasswordService>();
services.AddScoped<IAuthenticationService, AuthenticationService>();
services.AddScoped<IIndividualRepository, IndividualRepository>();
services.AddScoped<ITransactionRepository, TransactionRepository>();

services.AddScoped<IUserService, UserService>();
services.AddScoped<IBankAccountRepository, BankAccountRepository>();
services.AddScoped<IBankAccountService, BankAccountService>();
services.AddScoped<IEmployeeRepository, EmployeeRepository>();
services.AddScoped<IFixedDepositsRepository, FixedDepositsRepository>();
services.AddScoped<IOrganizationRepository, OrganizationRepository>();
services.AddScoped<IReportsRepository, ReportsRepository>();
services.AddScoped<IReportsService, ReportsService>();
services.AddScoped<IBankRepository, BankRepository>();

services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
services.AddSession();

// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-6.0
// https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims?view=aspnetcore-7.0
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

// add authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.NonCustomerPolicy, policy =>
    {
        // if user's role claim is not Customer, then allow access
        policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                c.Type == ClaimTypes.Role && c.Value != UserType.Customer.ToString()));
    });

    options.AddPolicy(Policies.CustomerPolicy, policy =>
    {
        // if user's role claim is Customer or Admin, then allow access
        policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                c.Type == ClaimTypes.Role && (c.Value == UserType.Customer.ToString() ||
                                              c.Value == UserType.Admin.ToString())));
    });
    
    options.AddPolicy(Policies.AdminPolicy, policy =>
    {
        // if user's role claim is Admin, then allow access
        policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                c.Type == ClaimTypes.Role && c.Value == UserType.Admin.ToString()));
    });
    
    options.AddPolicy(Policies.EmployeePolicy, policy =>
    {
        // if user's role claim is Employee, Manager or Admin, then allow access
        policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                c.Type == ClaimTypes.Role && (c.Value == UserType.Employee.ToString() ||
                c.Value == UserType.Manager.ToString() ||
                c.Value == UserType.Admin.ToString())));
    });
    
    options.AddPolicy(Policies.ManagerPolicy, policy =>
    {
        // if user's role claim is Manager or Admin, then allow access
        policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                c.Type == ClaimTypes.Role && (c.Value == UserType.Manager.ToString() ||
                                              c.Value == UserType.Admin.ToString())));
    });
});

services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/AccessDenied", "?statusCode={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();
app.MapDefaultControllerRoute();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
