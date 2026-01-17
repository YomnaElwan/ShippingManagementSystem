using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Domain.IUnitWorks;
using System.Collections.Immutable;

namespace ShippingSystem.Presentation.Controllers
{
    public class PaymentMethodController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public PaymentMethodController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Authorize(Policy= "ViewPaymentMethods")]
        public async Task<IActionResult> Index()
        {
            List<PaymentMethods> paymentMethodList = await unitOfWork.PaymentMethodsRepository.GetAllAsync();
            return View("Index",paymentMethodList);
        }
        [HttpGet]
        [Authorize(Policy = "AddNewPaymentMethod")]
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
                await unitOfWork.PaymentMethodsRepository.AddAsync(newPaymentMethod);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Add",newPaymentMethod);
        }
        [HttpGet]
        [Authorize(Policy = "EditPaymentMethod")]
        public async Task<IActionResult> Edit(int Id)
        {
            PaymentMethods paymentMethodFromDB = await unitOfWork.PaymentMethodsRepository.GetByIdAsync(Id);
            return View("Edit",paymentMethodFromDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(PaymentMethods editedPaymentMethod)
        {
            if (ModelState.IsValid)
            {
                await unitOfWork.PaymentMethodsRepository.UpdateAsync(editedPaymentMethod);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Edit",editedPaymentMethod);
        }
        [Authorize(Policy = "DeletePaymentMethod")]
        public async Task<IActionResult> Delete(int Id)
        {
            await unitOfWork.PaymentMethodsRepository.DeleteAsync(Id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
