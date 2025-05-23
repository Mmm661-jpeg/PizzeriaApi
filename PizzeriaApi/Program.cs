using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Core.Services;
using PizzeriaApi.Data.DataModels;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Data.Repository;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.UtilModels;
using PizzeriaApi.Extensions;
using Microsoft.Extensions.Logging.AzureAppServices;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.AddConsole();   
builder.Logging.AddDebug();      
builder.Logging.AddAzureWebAppDiagnostics(); 

builder.Logging.SetMinimumLevel(LogLevel.Information);



var keyVaultUrl = builder.Configuration["KeyVault:Url"];

var keyVaultEndpoint = Environment.GetEnvironmentVariable("AZURE_KEYVAULT_ENDPOINT");

if (!string.IsNullOrEmpty(keyVaultEndpoint))
{
    keyVaultUrl = keyVaultEndpoint;
}

if (!string.IsNullOrEmpty(keyVaultUrl))
{
    try
    {
        builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
        Console.WriteLine($"Successfully connected to Key Vault at {keyVaultUrl}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to connect to Key Vault: {ex.Message}");
    }
}
else
{
    Console.WriteLine("No Key Vault URL found in configuration");
}

//if (!string.IsNullOrEmpty(keyVaultUrl))
//{
//    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
//}

var connectionString = builder.Configuration["ConnectionString"];


if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Warning: Connection string is null or empty");
}
else
{
    Console.WriteLine("Connection string retrieved successfully");
}

//async Task SeedRoles(IServiceProvider serviceProvider)
//{
//    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//    string[] roleNames = { "RegularUser", "PremiumUser", "Admin" };

//    foreach (var roleName in roleNames)
//    {
//        var roleExists = await roleManager.RoleExistsAsync(roleName);
//        if (!roleExists)
//        {
//            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
//            if (!result.Succeeded)
//            {
//                throw new Exception($"Failed to create role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
//            }
//        }
//    }
//}

// Add services to the container.

builder.Services.AddDbContext<PizzeriaApiDBContext>(options =>
options.UseSqlServer(connectionString));

builder.Services.AddIdentity<PizzeriaUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Customize as needed
})
.AddEntityFrameworkStores<PizzeriaApiDBContext>()
.AddRoles<IdentityRole>()
.AddDefaultTokenProviders();



builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettingsRaw = builder.Configuration.GetSection("JwtSettings");
var issuer = jwtSettingsRaw["Issuer"];
var audience = jwtSettingsRaw["Audience"];
var signingKey = jwtSettingsRaw["Secret"];



Console.WriteLine($"JWT Issuer: {issuer}");
Console.WriteLine($"JWT Audience: {audience}");
Console.WriteLine("JWT Secret " + (string.IsNullOrEmpty(signingKey) ? "not found" : "found"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));

    // Authorization policies for role-based access control.
    // Usage: [Authorize(Policy = "Admin")] on controller/actions
    // Currently unused, but ready for future access restrictions.
    // Policies can be added for more than roles you can make custom ones like age
    // You can also [Authorize(Policy = "CustomPolicyName")] on controller/actions
    // For example over 18 policy then you can check the age claim and if it is over 18 then allow access
});

builder.Services.AddAuthenticationExtension(issuer, audience, signingKey);


builder.Services.AddControllers();

builder.Services.AddServicesExtension();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExtended();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwaggerExtended();
//}

app.UseSwaggerExtended();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    await SeedRoles(services);
//}



await app.RunAsync();


