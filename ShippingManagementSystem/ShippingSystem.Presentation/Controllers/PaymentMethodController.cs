using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using System.Collections.Immutable;

namespace ShippingSystem.Presentation.Controllers
{
    public class PaymentMethodController : Controller
    {
        private readonly IGenericService<PaymentMethod> paymentMethodService;
        public PaymentMethodController(IGenericService<PaymentMethod> paymentMethodService)
        {
            this.paymentMethodService = paymentMethodService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<PaymentMethod> paymentMethodList = await paymentMethodService.GetAllAsync();
            return View("Index",paymentMethodList);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAdd(PaymentMethod newPaymentMethod)
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
            PaymentMethod paymentMethodFromDB = await paymentMethodService.GetByIdAsync(Id);
            return View("Edit",paymentMethodFromDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(PaymentMethod editedPaymentMethod)
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
