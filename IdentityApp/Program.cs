using IdentityApp.Data;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Buffers.Text;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(
    opt=>opt.UseSqlite(builder.Configuration["ConnectionStrings:SQLite_Connection"]));
builder.Services.AddIdentity<AppUser,AppRole>().AddEntityFrameworkStores<ApplicationDbContext>();

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
#region Authentication
builder.Services.ConfigureApplicationCookie(opt =>
{//https://learn.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-8.0
    opt.LoginPath = "/Account/Login"; //Default degerler yerine baska url de kullanilabilir.
    opt.AccessDeniedPath = "/";//Belirli bir role sahip olan kullanicilari yetkili olduklari sayfaya yönlendirir. Onun disindaki kullanicilara ise erisiminin olmadigini belirtir.
    opt.SlidingExpiration = true;
    opt.ExpireTimeSpan = TimeSpan.FromDays(30);//Bir cookie 30 gün boyunca kayitli olacaktir sinir asimi oldugunda tekrar giris ister. Default deger 14 gündür.
    
    /*  Authentication
    - Cookie Based Authentication : Browser lerde kullanilan.
    - Token Based Authentication-JWT : Uygulamada üretilen Token Base kullanici her talep ettiginde kullaniciya gönderrilir.
                                        Json Web Token olarak adlandirlir.Bu tokenn Mobil uygulamalarda kullanilir.
    - External Provider Authentication : 
      seklinde 3 yöntemle yapilabilir.
  */
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

//Authentication aktifletirmek icin uygulamaya tanimaliyiz.
app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

SeedData.IdentityTestUser(app);//test verilerinin yüklenmesi

app.Run();
