using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductMSystem.Data;
using ProductMSystem.Data.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContextConnection")));


builder.Services.AddIdentity<IdentityUser, IdentityRole>()

    .AddEntityFrameworkStores<ApplicationDbContext>()

    .AddDefaultTokenProviders();

//Configuring services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ISuperAdminService, SuperAdminService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())

{

    var serviceProvider = scope.ServiceProvider;

    SeedData.Initialize(serviceProvider);
  

}


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
