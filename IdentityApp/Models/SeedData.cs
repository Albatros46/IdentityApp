using IdentityApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Models
{
    public class SeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Admin_123";
        
        public static async void IdentityTestUser(IApplicationBuilder app)
        {
            var context=app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (context.Database.GetAppliedMigrations().Any())
            {
                context.Database.Migrate();
            }

            var userManager=app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();

           var user= await userManager.FindByNameAsync(adminUser);
            if (user == null)
            {
                user = new AppUser
                {
                    FullName = "Servet Akcadag",
                    UserName = adminUser,
                    Email = "admin@gmail.com",
                    PhoneNumber="017641534152"
                };
                await userManager.CreateAsync(user,adminPassword);

            }
        }
    }
}
