using System.Security.Claims;
using Atlanta.DAL;
using Atlanta.DAL.Interfaces;
using Atlanta.DAL.Repositories;
using Atlanta.Domain.Services;
using Atlanta.Services.Interfaces;
using Atlanta.Services.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(connection));
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoomService, RoomService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
        opt.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", opt => opt.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("User", opt => opt.RequireClaim(ClaimTypes.Role, "User"));
    options.AddPolicy("RoomMan", opt => opt.RequireClaim(ClaimTypes.Role, "RoomMan"));
    options.AddPolicy("StaffMan", opt => opt.RequireClaim(ClaimTypes.Role, "StaffMan"));
});
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();