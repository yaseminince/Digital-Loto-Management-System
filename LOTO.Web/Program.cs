using LOTO.Application.Interfaces;
using LOTO.Application.Services;
using LOTO.Domain.Entities;
using LOTO.Infrastructure.EFCore;
using LOTO.Infrastructure.Utils;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.DependencyInjection();

builder.Services.AddIdentity<User, Role>() // user ve role identity ile belirlenir 
    .AddEntityFrameworkStores<LotoContext>() // identitynin bilgileri lotocontext'e saklanır 
    .AddDefaultTokenProviders(); // şifre sıfırlama , mail doğrulama işlemleri 

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/AccessDenied";
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRightService, RightService>();
builder.Services.AddScoped<ILotoService, LotoService>();
builder.Services.AddScoped<IPlantService, PlantService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();