using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels.PermissionsVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class PermissionsController : Controller
    {
        private readonly RoleManager<IdentityRole> allRoles;
        private readonly IGenericService<PermissionsModule> allModules;
        private readonly IGenericService<RolePermissions> rolePermissions;
        private readonly IMapper _mapper;
        public PermissionsController(RoleManager<IdentityRole> allRoles,
                                     IGenericService<PermissionsModule> allModules,
                                     IGenericService<RolePermissions> rolePermissions,
                                     IMapper _mapper)
        {
            this.allRoles = allRoles;
            this.allModules = allModules;
            this.rolePermissions = rolePermissions;
            this._mapper = _mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Content("Role Permissions are added successfully");
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            List<IdentityRole> rolesList = allRoles.Roles.ToList();
            List<PermissionsModule> modulesList =await allModules.GetAllAsync();
            ModulePermissionsVM modulePermissions = new ModulePermissionsVM()
            {
                Modules = modulesList,
                Roles = rolesList
            };
            return View("Add",modulePermissions);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAdd(ModulePermissionsVM newModulePermissionsVM)
        {
            if (string.IsNullOrEmpty(newModulePermissionsVM.RoleId))
            {
                ModelState.AddModelError("RoleId", "You Must Choose a Role!");
            }
            if (ModelState.IsValid)
            {
                foreach(var item in newModulePermissionsVM.ModulePerms)
                {
                    RolePermissions role = new RolePermissions() {
                        RoleId=newModulePermissionsVM.RoleId,
                        PermissionsModuleId=item.ModuleId,
                        CanView=item.CanView,
                        CanViewDetails=item.CanViewDetails,
                        CanAdd=item.CanAdd,
                        CanEdit=item.CanEdit,
                        CanDelete=item.CanDelete,
                    };
                    await rolePermissions.AddAsync(role);
                }
                await rolePermissions.SaveAsync();
                return RedirectToAction("Index");
            }
            newModulePermissionsVM.Modules = await allModules.GetAllAsync();
            newModulePermissionsVM.Roles = allRoles.Roles.ToList();
            return View("Add",newModulePermissionsVM);
        }
    }
}
