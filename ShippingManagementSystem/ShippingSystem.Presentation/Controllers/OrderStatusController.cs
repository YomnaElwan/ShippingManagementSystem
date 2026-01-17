using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Presentation.ViewModels.OrderItemsVM;
using ShippingSystem.Presentation.ViewModels.OrderStatusVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class OrderStatusController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public OrderStatusController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Authorize(Policy = "ViewAllOrderStatus")]
        public async Task<IActionResult> Index(int pageNumber=1,int pageSize=5)
        {
            List<OrderStatus> orderStatusList = await unitOfWork.OrderStatusRepository.GetAllAsync();
            var totalItems = orderStatusList.Count();
            var allOrderSts = orderStatusList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = Math.Ceiling((double)totalItems / pageSize);
            return View("Index",allOrderSts);
        }
        [HttpGet]
        [Authorize(Policy = "AddOrderStatus")]
        public IActionResult Add()
        {
            return View("Add");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> SaveAdd(AddOrderStatusVM newStatus)
        {
            if (ModelState.IsValid)
            {
                OrderStatus newOrderSts = new OrderStatus() { 
                Name=newStatus.StsName
                };
                await unitOfWork.OrderStatusRepository.AddAsync(newOrderSts);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
             
            }
            return View("Add",newStatus);
        }
        [HttpGet]
        [Authorize(Policy = "EditOrderStatus")]
        public async Task<IActionResult> Edit(int Id)
        {
            OrderStatus orderStatusFromDB = await unitOfWork.OrderStatusRepository.GetByIdAsync(Id);
            EditOrderStatusVM editOrderStatus = new EditOrderStatusVM()
            {
                Id = orderStatusFromDB.Id,
                StsName = orderStatusFromDB.Name
            };      
            return View("Edit",editOrderStatus);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditOrderStatusVM editFromUser)
        {
            if (ModelState.IsValid) {
                OrderStatus orderStatusFromDatabase = await unitOfWork.OrderStatusRepository.GetByIdAsync(editFromUser.Id);
                orderStatusFromDatabase.Name = editFromUser.StsName;
                await unitOfWork.OrderStatusRepository.UpdateAsync(orderStatusFromDatabase);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Edit",editFromUser);
        }
        [Authorize(Policy = "DeleteOrderStatus")]
        public async Task<IActionResult>Delete(int Id)
        {
            await unitOfWork.OrderStatusRepository.DeleteAsync(Id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
