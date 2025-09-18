using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels.OrderItemsVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly IGenericService<OrderItem> orderItemsService;
        public OrderItemsController(IGenericService<OrderItem> orderItemsService)
        {
            this.orderItemsService = orderItemsService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<OrderItem> orderItemsList = await orderItemsService.GetAllAsync();
            List<GetOrderItemsVM> orderItemsVM = orderItemsList.Select(o => new GetOrderItemsVM { 
            ProductName=o.ProductName,
                Quantity = o.Quantity,
                Weight = o.Weight,
                Price = o.Price,
                Id=o.Id
            }).ToList();
            return PartialView("Index", orderItemsVM);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View("Add");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAdd(AddOrderItemsVM newOrderItemsVM)
        {
            if (ModelState.IsValid)
            {
                OrderItem newOrderItems = new OrderItem()
                {
                    ProductName = newOrderItemsVM.ProductName,
                    Quantity = newOrderItemsVM.Quantity,
                    Weight = newOrderItemsVM.Weight,
                    Price=newOrderItemsVM.Price,
                    OrderId = newOrderItemsVM.OrderId
                };
                await orderItemsService.AddAsync(newOrderItems);
                await orderItemsService.SaveAsync();
            }
            return View("Add",newOrderItemsVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            return View("Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit()
        {
            return View("Edit");
        }
        public async Task<IActionResult> Delete(int Id)
        {

            return RedirectToAction("Index");
        }

    }
}
