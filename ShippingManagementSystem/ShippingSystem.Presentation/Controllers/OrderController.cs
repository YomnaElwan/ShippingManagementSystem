using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mono.TextTemplating;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Application.Services;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Presentation.SessionExtensions;
using ShippingSystem.Presentation.ViewModels.OrderItemsVM;
using ShippingSystem.Presentation.ViewModels.OrderVM;
using System.Security.Claims;

namespace ShippingSystem.Presentation.Controllers
{
    public class OrderController : Controller
    {
        private readonly IGenericService<Governorates> govService;
        private readonly IGenericService<Branches> branchService;
        private readonly IGenericService<ShippingType> shippingTypeService;
        private readonly IGenericService<PaymentMethod> paymentMethodService;
        private readonly ICityService cityService;
        private readonly IGenericService<Orders> orderService;
        private readonly IGenericService<OrderItem> orderItemsService;
        private readonly IMerchantService merchantService;
        public OrderController(IGenericService<Governorates> govService,
                               IGenericService<Branches> branchService,
                               IGenericService<ShippingType> shippingTypeService,
                               IGenericService<PaymentMethod> paymentMethodService,
                               ICityService cityService,
                               IGenericService<Orders> orderService,
                               IGenericService<OrderItem> orderItemsService,
                               IMerchantService merchantService
                               )
        {
            this.govService = govService;
            this.branchService = branchService;
            this.shippingTypeService = shippingTypeService;
            this.paymentMethodService = paymentMethodService;
            this.cityService = cityService;
            this.orderService = orderService;
            this.orderItemsService = orderItemsService;
            this.merchantService = merchantService;
        }
        [HttpPost]
        public IActionResult AddProductToOrder(AddOrderItemsVM product)
        {
            // استرجاع المنتجات من السيشن أو إنشاء List جديدة
            List<GetOrderItemsVM> items = HttpContext.Session.GetObjectFromJson<List<GetOrderItemsVM>>("OrderItems")
                                         ?? new List<GetOrderItemsVM>();

            // إضافة المنتج الجديد
            items.Add(new GetOrderItemsVM
            {
                Id = items.Count + 1,
                ProductName = product.ProductName,
                Quantity = product.Quantity,
                Weight = product.Weight,
                Price = product.Price
            });

            // تحديث السيشن
            HttpContext.Session.SetObjectAsJson("OrderItems", items);

            // إرجاع Partial جديد بالجدول
            return PartialView("_GetOrderItemsPartial", items);
        }


        [HttpGet]
        public async Task<IActionResult>GetCityList(int govId)
        {
            List<Cities> cityList = await cityService.cityListByGov(govId);
            return Json(cityList);
        }
        [HttpGet]
        public IActionResult AddProductForm()
        {
            return PartialView("_AddProductForm", new AddOrderItemsVM());
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            AddOrderVM addOrderVM = new AddOrderVM()
            {
                GovList = await govService.GetAllAsync(),
                BranchList = await branchService.GetAllAsync(),
                ShippingTypeList = await shippingTypeService.GetAllAsync(),
                PaymentMethodList = await paymentMethodService.GetAllAsync(),
                CityList = new List<Cities>(),
                Merchants = await merchantService.SpecialMerchantsList()

            };
            return View("Add",addOrderVM);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SaveAdd(AddOrderVM newOrderFromUser)
        //{
        //    if (ModelState.IsValid) {
        //        Orders newOrder = new Orders()
        //        {
        //            CustomerName = newOrderFromUser.CustomerName,
        //            CustomerEmail = newOrderFromUser.CustomerEmail,
        //            BranchId = newOrderFromUser.BranchId,
        //            GovernorateId = newOrderFromUser.GovernorateId,
        //            CityId = newOrderFromUser.CityId,
        //            Address = newOrderFromUser.Address,
        //            PhoneNumber1 = newOrderFromUser.PhoneNumber1,
        //            PhoneNumber2 = newOrderFromUser.PhoneNumber2,
        //            DeliveryTypeOption = newOrderFromUser.DeliveryTypeOption,
        //            ShippingTypeId = newOrderFromUser.ShippingTypeId,
        //            PaymentMethodId = newOrderFromUser.PaymentMethodId,
        //            TotalCost = newOrderFromUser.TotalCost,
        //            TotalWeight = newOrderFromUser.TotalWeight,
        //            MerchantId = newOrderFromUser.MerchantId,
        //            Notes = newOrderFromUser.Notes,

        //        };
        //        await orderService.AddAsync(newOrder);
        //        await orderService.SaveAsync();

        //        foreach (var item in newOrderFromUser.OrderItems)
        //        {
        //            OrderItem newOrderItems = new OrderItem()
        //            {
        //                ProductName = item.ProductName,
        //                Quantity = item.Quantity,
        //                OrderId = newOrder.Id,
        //                Weight = item.Weight,
        //                Price = item.Price,
        //            };
        //            await orderItemsService.AddAsync(newOrderItems);

        //        }
        //        await orderItemsService.SaveAsync();

        //        return RedirectToAction("Index", "Home");
        //    }


        //    return View("Add",newOrderFromUser);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAdd(AddOrderVM newOrderFromUser)
        {
            if (ModelState.IsValid)
            {
                var orderItems = HttpContext.Session.GetObjectFromJson<List<GetOrderItemsVM>>("OrderItems");

                decimal totalCost = orderItems.Sum(item => item.Price * item.Quantity);
                decimal totalWeight = orderItems.Sum(item => item.Weight * item.Quantity);

                var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
                var merchant = (await merchantService.GetAllAsync())
                .FirstOrDefault(m => m.UserId == userId);

                Orders newOrder = new Orders()
                {
                    CustomerName = newOrderFromUser.CustomerName,
                    CustomerEmail = newOrderFromUser.CustomerEmail,
                    BranchId = newOrderFromUser.BranchId,
                    GovernorateId = newOrderFromUser.GovernorateId,
                    CityId = newOrderFromUser.CityId,
                    Address = newOrderFromUser.Address,
                    PhoneNumber1 = newOrderFromUser.PhoneNumber1,
                    PhoneNumber2 = newOrderFromUser.PhoneNumber2,
                    DeliveryTypeOption = newOrderFromUser.DeliveryTypeOption,
                    ShippingTypeId = newOrderFromUser.ShippingTypeId,
                    PaymentMethodId = newOrderFromUser.PaymentMethodId,
                    TotalCost = totalCost,
                    TotalWeight = totalWeight ,
                    MerchantId = merchant.Id,
                    Notes = newOrderFromUser.Notes,
                };

                await orderService.AddAsync(newOrder);
                await orderService.SaveAsync();

                // ✅ هنا بنسحب الـ Items من السيشن

                if (orderItems != null && orderItems.Any())
                {
                    foreach (var item in orderItems)
                    {
                        OrderItem newOrderItems = new OrderItem()
                        {
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            OrderId = newOrder.Id,
                            Weight = item.Weight,
                            Price = item.Price,
                        };
                        await orderItemsService.AddAsync(newOrderItems);
                    }

                    await orderItemsService.SaveAsync();
                }

                // بعد الحفظ، امسحي السيشن
                HttpContext.Session.Remove("OrderItems");

                return RedirectToAction("Index", "Home");
            }

            return View("Add", newOrderFromUser);
        }

    }
}
