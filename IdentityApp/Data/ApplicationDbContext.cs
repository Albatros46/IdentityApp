using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,AppRole,string>//string olarak otomatik prola üretilecek
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //bu yontem ile connectionstring disaridan alinir. appsettings.json icinde tanimlanir.
        }
        //diger yöntem
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=IdentityDb");
            base.OnConfiguring(optionsBuilder);
        }
        */
    }
}
