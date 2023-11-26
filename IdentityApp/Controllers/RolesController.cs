using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityApp.Controllers
{
    public class RolesController : Controller
    {//https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.rolemanager-1?view=aspnetcore-7.0
        
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AppRole model)
        {
            if (ModelState.IsValid)
            {
                var result=await _roleManager.CreateAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }
            return View(model);
        }
    
        [HttpGet]
        public async Task<IActionResult> Edit(string  id)
        {
            var role=await _roleManager.FindByIdAsync(id);
            if (role!=null && role.Name!=null)
            {
                ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);
                return View(role);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppRole model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);

                if (role!=null)
                {
                    role.Name = model.Name;
                    var result=await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("",err.Description);
                    }
                    if(role.Name!=null)
                        ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }
    }
}
