using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;

namespace ShippingSystem.Presentation.Controllers
{
    public class ShippingTypeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public ShippingTypeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Authorize(Policy = "ViewShippingTypes")]
        public async Task<IActionResult> Index()
        {
            List<ShippingTypes> shippingTypeList = await unitOfWork.ShippingTypesRepository.GetAllAsync();
            return View("Index",shippingTypeList);
        }
        [HttpGet]
        [Authorize(Policy = "AddNewShippingType")]
        public IActionResult Add()
        {
            return View("Add");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAdd(ShippingTypes newType)
        {
            if (ModelState.IsValid)
            {
                await unitOfWork.ShippingTypesRepository.AddAsync(newType);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Add",newType);
        }

        [HttpGet]
        [Authorize(Policy = "EditShippingType")]
        public async Task<IActionResult> Edit(int Id)
        {
            var shippingTypeFromDB = await unitOfWork.ShippingTypesRepository.GetByIdAsync(Id);
            return View("Edit",shippingTypeFromDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(ShippingTypes editedShippingType)
        {
            if (ModelState.IsValid)
            {
                await unitOfWork.ShippingTypesRepository.UpdateAsync(editedShippingType);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Edit",editedShippingType);
        }
        [Authorize(Policy = "DeleteShippingType")]
        public async Task<IActionResult> Delete(int Id)
        {
            await unitOfWork.ShippingTypesRepository.DeleteAsync(Id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
