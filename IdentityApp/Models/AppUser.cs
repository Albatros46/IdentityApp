using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Models
{
    public class AppUser:IdentityUser
    {//IdentityUser e ait diger bilgileri kullanmak icin bu class a ihtiyacimiz var.
        //daha sonra program.cs de bu class in tanimlanmasi gerekir.
        public string? FullName { get; set; }

    }
}
