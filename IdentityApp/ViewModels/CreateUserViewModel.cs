using System.ComponentModel.DataAnnotations;

namespace IdentityApp.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage ="Kullanici adini giriniz!")]
        public string UserName { get; set; } = string.Empty; //veya string? seklinde tanimlanir.

        [Required(ErrorMessage = "E-Mail adres giriniz!")]
        [EmailAddress]
        public string Email { get; set; }= string.Empty;

        [Required(ErrorMessage = "Parolayi belirleyin!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }= string.Empty;

        [Required(ErrorMessage = "Parolayi onaylayin!")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Parola uyusmuyor!")]//compare karsilastima icin kullanilir parola ile confirmpassword karsilastirma
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
