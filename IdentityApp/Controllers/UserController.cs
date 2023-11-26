using IdentityApp.Models;
using IdentityApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Controllers
{
    public class UserController : Controller
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }
        public IActionResult Create()
        {//create formunu gostermek icin
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser{
                    UserName= model.Email,
                    Email=model.Email,
                    FullName=model.FullName
                };
                IdentityResult result =await _userManager.CreateAsync(user,model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }
                
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
          
            if (id==null)
            {
                return RedirectToAction("Index");

            }
            var user =await _userManager.FindByIdAsync(id);
            if (user!=null)
            {
                //rollero controller den view e tasiyacagiz
                ViewBag.Roles = await _roleManager.Roles.Select(i=>i.Name).ToListAsync();
                return View(new EditUserViewModel
                {
                    Id = user.Id,
                    FullName=user.FullName,
                    Email=user.Email,
                    SelectedRoles=await _userManager.GetRolesAsync(user),
                });

            }//eger böyle bir kullanici yoksa tekrar indexe yönlendirilir.
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task< IActionResult> Edit(EditUserViewModel model,string id)
        {//route den gelen id ile url den gelen id karsilastiracagiz
            if (id!=model.Id)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var user=await _userManager.FindByIdAsync (model.Id);
                if (user!=null)
                {
                    user.Email = model.Email;
                    user.FullName = model.FullName;
                    var result=await _userManager.UpdateAsync(user);
                    if (result.Succeeded && !string.IsNullOrEmpty(model.Password))
                    {
                        await _userManager.RemovePasswordAsync(user);//parolasini silecegiz admin olarak
                        await _userManager.AddPasswordAsync(user, model.Password);//formdana gelen parolayi yeni parola gibi gonderecegiz
                    }
                    if (result.Succeeded)
                    {//eger result Succesd ise kulaniciyi  güncelle ve index e yönlendir
                        await _userManager.RemoveFromRolesAsync(user,await _userManager.GetRolesAsync(user));
                        if (model.SelectedRoles!=null)
                        {
                            await _userManager.AddToRolesAsync(user, model.SelectedRoles);

                        }
                        return RedirectToAction("Index");
                    }//eger kullanici success degilse hata göster ekranda
                    foreach (IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("",err.Description);//edit formu model seviyesindeki hatalarda burada kaynaklanan hatalari da gosterecek
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user!=null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}
