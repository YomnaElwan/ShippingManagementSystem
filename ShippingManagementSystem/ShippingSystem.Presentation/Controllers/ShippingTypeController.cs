using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;

namespace ShippingSystem.Presentation.Controllers
{
    public class ShippingTypeController : Controller
    {
        private readonly IGenericService<ShippingType> _shippingTypeService;
        public ShippingTypeController(IGenericService<ShippingType> _shippingTypeService)
        {
            this._shippingTypeService = _shippingTypeService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<ShippingType> shippingTypeList = await _shippingTypeService.GetAllAsync();
            return View("Index",shippingTypeList);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAdd(ShippingType newType)
        {
            if (ModelState.IsValid)
            {
                await _shippingTypeService.AddAsync(newType);
                await _shippingTypeService.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Add",newType);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var shippingTypeFromDB = await _shippingTypeService.GetByIdAsync(Id);
            return View("Edit",shippingTypeFromDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(ShippingType editedShippingType)
        {
            if (ModelState.IsValid)
            {
                await _shippingTypeService.UpdateAsync(editedShippingType);
                await _shippingTypeService.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Edit",editedShippingType);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            await _shippingTypeService.DeleteAsync(Id);
            await _shippingTypeService.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
