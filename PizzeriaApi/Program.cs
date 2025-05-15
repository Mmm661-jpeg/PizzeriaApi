using Microsoft.EntityFrameworkCore;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Core.Services;
using PizzeriaApi.Data.DataModels;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Data.Repository;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.UtilModels;
using PizzeriaApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<PizzeriaApiDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<PizzeriaUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Customize as needed
})
.AddEntityFrameworkStores<PizzeriaApiDBContext>();


builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettingsRaw = builder.Configuration.GetSection("JwtSettings");
var issuer = jwtSettingsRaw["Issuer"];
var audience = jwtSettingsRaw["Audience"];
var signingKey = jwtSettingsRaw["Secret"];

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

//services and repos-------------

builder.Services.AddScoped<IPizzeriaUserRepo, PizzeriaUserRepo>();
builder.Services.AddScoped<IPizzeriaUserService, PizzeriaUserService>();

builder.Services.AddScoped<IDishIngredientsRepo, DishIngredientsRepo>();
builder.Services.AddScoped<IDishIngredientsService, DishIngredientsService>();

builder.Services.AddScoped<IDishesRepo, DishesRepo>();
builder.Services.AddScoped<IDishesService, DishesService>();

builder.Services.AddScoped<IOrdersRepo, OrdersRepo>();
builder.Services.AddScoped<IOrdersService, OrdersService>();

builder.Services.AddScoped<IOrderItemsRepo,OrderItemsRepo>();
builder.Services.AddScoped<IOrderItemsService, OrderItemsService>();

builder.Services.AddScoped<IIngredientsRepo, IngredientsRepo>();
builder.Services.AddScoped<IIngredientsService, IngredientsService>();

builder.Services.AddScoped<ICategoriesRepo,CategoriesRepo>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();

builder.Services.AddTransient<ITokenGenerator, TokenGenerator>();

//Services and repos-----------------




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExtended();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
