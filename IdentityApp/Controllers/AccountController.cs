using IdentityApp.Models;
using IdentityApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;
        private SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
           if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user!=null)
                {
                    await _signInManager.SignOutAsync();//cikis yapmis ise tarayicidan Cookie silinecek.

                    var result = await _signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,true);//Program.cs de belirttigimizgecikme süresi aktif olsun.
                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user,null);//Ne kadar süre sonra SetLockout silinsin. null deger yerine bir tarif verilebilir.

                        return RedirectToAction("Index", "Roles");
                    }
                    else if (result.IsLockedOut)
                    {
                        var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeleft = lockoutDate.Value - DateTime.UtcNow;
                        ModelState.AddModelError("",$"Hesabiniz kilitlendi. Lütfen {timeleft.Minutes} dakika sonra deneyiniz!");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Parolaniz hatali!");

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu e-mail adresi ile hesap bulunamadi!");
                }
            }
            return View(model);
        }
    }
}
