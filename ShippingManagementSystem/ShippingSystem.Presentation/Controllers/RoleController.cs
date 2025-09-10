using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Presentation.ViewModels.RoleVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> _roleManager)
        {
            this._roleManager = _roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<IdentityRole> roleList = await _roleManager.Roles.ToListAsync();
            return View("Index",roleList);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoleViewModel roleView)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole() { 
                       Name=roleView.RoleName,
                };
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index","Role");
                     
                }
                else
                {
                    foreach(var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View("Add", roleView);
           
        }

    }
}
