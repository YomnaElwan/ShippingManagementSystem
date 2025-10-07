using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using System.Collections.Immutable;

namespace ShippingSystem.Presentation.Controllers
{
    public class PaymentMethodController : Controller
    {
        private readonly IGenericService<PaymentMethods> paymentMethodService;
        public PaymentMethodController(IGenericService<PaymentMethods> paymentMethodService)
        {
            this.paymentMethodService = paymentMethodService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<PaymentMethods> paymentMethodList = await paymentMethodService.GetAllAsync();
            return View("Index",paymentMethodList);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAdd(PaymentMethods newPaymentMethod)
        {
            if (ModelState.IsValid)
            {
                await paymentMethodService.AddAsync(newPaymentMethod);
                await paymentMethodService.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Add",newPaymentMethod);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            PaymentMethods paymentMethodFromDB = await paymentMethodService.GetByIdAsync(Id);
            return View("Edit",paymentMethodFromDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(PaymentMethods editedPaymentMethod)
        {
            if (ModelState.IsValid)
            {
                await paymentMethodService.UpdateAsync(editedPaymentMethod);
                await paymentMethodService.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Edit",editedPaymentMethod);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            await paymentMethodService.DeleteAsync(Id);
            await paymentMethodService.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
