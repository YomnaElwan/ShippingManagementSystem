using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels.OrderItemsVM;
using ShippingSystem.Presentation.ViewModels.OrderStatusVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class OrderStatusController : Controller
    {
        private readonly IGenericService<OrderStatus> orderStatusService;
        public OrderStatusController(IGenericService<OrderStatus> orderStatusService)
        {
            this.orderStatusService = orderStatusService;
        }
        [HttpGet]
        [Authorize(Policy = "ViewAllOrderStatus")]
        public async Task<IActionResult> Index(int pageNumber=1,int pageSize=5)
        {
            List<OrderStatus> orderStatusList = await orderStatusService.GetAllAsync();
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
                await orderStatusService.AddAsync(newOrderSts);
                await orderStatusService.SaveAsync();
                return RedirectToAction("Index");
             
            }
            return View("Add",newStatus);
        }
        [HttpGet]
        [Authorize(Policy = "EditOrderStatus")]
        public async Task<IActionResult> Edit(int Id)
        {
            OrderStatus orderStatusFromDB = await orderStatusService.GetByIdAsync(Id);
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
                OrderStatus orderStatusFromDatabase = await orderStatusService.GetByIdAsync(editFromUser.Id);
                orderStatusFromDatabase.Name = editFromUser.StsName;
                await orderStatusService.UpdateAsync(orderStatusFromDatabase);
                await orderStatusService.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Edit",editFromUser);
        }
        [Authorize(Policy = "DeleteOrderStatus")]
        public async Task<IActionResult>Delete(int Id)
        {
            await orderStatusService.DeleteAsync(Id);
            await orderStatusService.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
