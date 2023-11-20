using IdentityApp.Data;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(
    opt=>opt.UseSqlite(builder.Configuration["ConnectionStrings:SQLite_Connection"]));
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

#region IdentityOption
//Identity configurasyonunu burada yapabilirsiniz
//https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-8.0
builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 5;
    opt.Password.RequiredUniqueChars = 1;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;

    opt.Lockout.MaxFailedAccessAttempts = 5;//Maximum 5 defa hata yapabilsin kullanici daha 5 den sonra alt satirdaki islem gerceklessin
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);//eger giris basarisiz olursa 5 dakika kullanici kilitlenecek
   
});
#endregion


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

SeedData.IdentityTestUser(app);//test verilerinin yüklenmesi

app.Run();
