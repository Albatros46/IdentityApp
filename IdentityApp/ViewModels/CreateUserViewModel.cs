using System.ComponentModel.DataAnnotations;

namespace IdentityApp.ViewModels
{
    public class CreateUserViewModel
    {
        //[Display(Name = "User Name")]
        //[Required(ErrorMessage ="Kullanici adini giriniz!")]
        //public string UserName { get; set; } = string.Empty; //veya string? seklinde tanimlanir.

        [Display(Name ="Full Name")]
        public string FullName { get; set; } = string.Empty; //veya string? seklinde tanimlanir.

        [Required(ErrorMessage = "E-Mail adres giriniz!")]
        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }= string.Empty;

        [Required(ErrorMessage = "Parolayi belirleyin!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }= string.Empty;

        [Required(ErrorMessage = "Parolayi onaylayin!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password),ErrorMessage ="Parola uyusmuyor!")]//compare karsilastima icin kullanilir parola ile confirmpassword karsilastirma
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
