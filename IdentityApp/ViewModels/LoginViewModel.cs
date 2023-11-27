using System.ComponentModel.DataAnnotations;

namespace IdentityApp.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name ="E-Mail")]
        [EmailAddress]
        public string Email { get; set; } = null!;
       
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; } = true;
    }
}
