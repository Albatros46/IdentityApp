using System.ComponentModel.DataAnnotations;

namespace IdentityApp.ViewModels
{
    public class EditUserViewModel
    {
        public string? Id { get; set; }//Edit.cshtml de hidden olarak belirtilen yerde kullanici id tutulacak
        [Display(Name = "Full Name")]
        public string? FullName { get; set; } 

        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Parola uyusmuyor!")]//compare karsilastima icin kullanilir parola ile confirmpassword karsilastirma
        public string? ConfirmPassword { get; set; } 
    }
}
