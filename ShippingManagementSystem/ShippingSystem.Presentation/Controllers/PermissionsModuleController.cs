using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;

namespace ShippingSystem.Presentation.Controllers
{
    public class PermissionsModuleController : Controller
    {
        private readonly IGenericService<PermissionsModule> permissions;
        public PermissionsModuleController(IGenericService<PermissionsModule> permissions)
        {
            this.permissions = permissions;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<PermissionsModule> permissionsModules = await permissions.GetAllAsync();
            return View("Index",permissionsModules);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var permissionDetails = await permissions.GetByIdAsync(Id);
            return View("Details",permissionDetails);
        }
        [HttpGet]
        public IActionResult Add() {

            return View("Add");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SaveAdd(PermissionsModule permissionFromUser)
        {
            if (ModelState.IsValid)
            {
                await permissions.AddAsync(permissionFromUser);
                await permissions.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Add",permissionFromUser);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id) 
        {
            var getPermissionFromDB = await permissions.GetByIdAsync(Id);
            return View("Edit",getPermissionFromDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(PermissionsModule editPermission)
        {
            if (ModelState.IsValid)
            {
                await permissions.UpdateAsync(editPermission);
                await permissions.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Edit", editPermission);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            await permissions.DeleteAsync(Id);
            await permissions.SaveAsync();
            return RedirectToAction("Index");
        }
     
    }
}
