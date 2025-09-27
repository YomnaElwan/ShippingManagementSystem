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
        private readonly IGenericService<OrderStatus> orderStatusService;
        private readonly IOrderService customOrderService;
        public OrderController(IGenericService<Governorates> govService,
                               IGenericService<Branches> branchService,
                               IGenericService<ShippingType> shippingTypeService,
                               IGenericService<PaymentMethod> paymentMethodService,
                               ICityService cityService,
                               IGenericService<Orders> orderService,
                               IGenericService<OrderItem> orderItemsService,
                               IMerchantService merchantService,
                               IGenericService<OrderStatus> orderStatusService,
                               IOrderService customOrderService
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
            this.orderStatusService = orderStatusService;
            this.customOrderService = customOrderService;
        }
        //Get Orders List
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Orders> orderList = await customOrderService.GetSpecialOrderList();
            List<GetOrdersVM> mappedOrderList = orderList.Select(order => new GetOrdersVM
            {
                OrderId = order.Id,
                CreateAt = order.CreateAt,
                CustomerName = order.CustomerName,
                PhoneNumber1 = order.PhoneNumber1,
                GovernorateName = order.Governorate?.Name,
                CityName = order.City?.Name,
                TotalCost = order.TotalCost,
                StatusName = order.OrderStatus?.Name,

            }).ToList();
            return PartialView("_GetOrderIndexTable", mappedOrderList);
        }

        //Order List Based On Order Status
        [HttpGet]
        public async Task<IActionResult> GetOrderListByOrderStatus(int orderStsId)
        {
            List<Orders> orderListBySts = await customOrderService.GetOrdersByOrderStsId(orderStsId);
            List<GetOrdersVM> mappedOrderList = orderListBySts.Select(order => new GetOrdersVM
            {
                OrderId=order.Id,
                CreateAt=order.CreateAt,
                CustomerName=order.CustomerName,
                PhoneNumber1=order.PhoneNumber1,
                GovernorateName=order.Governorate?.Name,
                CityName=order.City?.Name,
                TotalCost=order.TotalCost,
                StatusName=order.OrderStatus?.Name,
            }).ToList();
            return PartialView("_GetOrderIndexTable",mappedOrderList);
        }
        [HttpGet]
        public async Task<IActionResult> IndexBasedOnSts()
        {
            List<OrderStatus> orderStatusList = await orderStatusService.GetAllAsync();
            return View("IndexBasedOnSts", orderStatusList);
        }
        
        //Edit Order Status
        [HttpGet]
        public async Task<IActionResult> EditOrderSts(int Id)
        {
            Orders orderFromDB = await customOrderService.GetOrderById(Id);
            if (orderFromDB == null)
            {
                return NotFound();
            };
            EditOrderStsVM mappingEditOrderSts = new EditOrderStsVM()
            {
                Id=orderFromDB.Id,
                OrderStsId = orderFromDB.OrderStatusId,
            };
            mappingEditOrderSts.OrderStsList = await orderStatusService.GetAllAsync();
            return View("EditOrderSts",mappingEditOrderSts);
        }

        //Save Edit Order Status To Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEditOrderSts(EditOrderStsVM editOrderStsFromUser)
        {
            if (ModelState.IsValid)
            {
                Orders orderFromDB = await customOrderService.GetOrderById(editOrderStsFromUser.Id);
                if (orderFromDB==null)
                {
                    return NotFound();
                }
                orderFromDB.OrderStatusId = editOrderStsFromUser.OrderStsId;
                await orderService.UpdateAsync(orderFromDB);
                await orderService.SaveAsync();
                return RedirectToAction("Index");
            }
            editOrderStsFromUser.OrderStsList = await orderStatusService.GetAllAsync();
            return View("EditOrderSts",editOrderStsFromUser);
        }

        //Add OrderItems to Order
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

        //Get City List Based On Governorate
        [HttpGet]
        public async Task<IActionResult>GetCityList(int govId)
        {
            List<Cities> cityList = await cityService.cityListByGov(govId);
            return Json(cityList);
        }

        //Get Add Products Form
        [HttpGet]
        public IActionResult AddProductForm()
        {
            return PartialView("_AddProductForm", new AddOrderItemsVM());
        }

        //Add New Order
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var merchant = (await merchantService.SpecialMerchantsList())
            .FirstOrDefault(m => m.UserId == userId);

            AddOrderVM addOrderVM = new AddOrderVM()
            {
                GovList = await govService.GetAllAsync(),
                BranchList = await branchService.GetAllAsync(),
                ShippingTypeList = await shippingTypeService.GetAllAsync(),
                PaymentMethodList = await paymentMethodService.GetAllAsync(),
                CityList = new List<Cities>(),
                OrderStatusList = await orderStatusService.GetAllAsync(),
                MerchantPhoneNum=merchant?.User?.PhoneNumber??"null",
                MerchantAddress=merchant?.User?.Address??"null",
            };
            return View("Add",addOrderVM);
        }

        //Save Order To Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAdd(AddOrderVM newOrderFromUser)
        {
            if (newOrderFromUser.OrderStatusId == 0)
            {
                ModelState.AddModelError("OrderStatusId", "You Must Select Order Status!");
            }
            if (newOrderFromUser.GovernorateId == 0)
            {
                ModelState.AddModelError("GovernorateId", "You Must Select a Governorate!");
            }
            if (newOrderFromUser.CityId==0)
            {
                ModelState.AddModelError("CityId", "You Must Select a City!");
            }
            if (newOrderFromUser.BranchId == 0)
            {
                ModelState.AddModelError("BranchId", "You Must Select a Branch!");
            }
            if (newOrderFromUser.PaymentMethodId == 0)
            {
                ModelState.AddModelError("PaymentMethodId", "You Must Select a Payment Method!");
            }
            if (newOrderFromUser.ShippingTypeId == 0)
            {
                ModelState.AddModelError("ShippingTypeId", "You Must Select a Shipping Type!");
            }
            if (newOrderFromUser.DeliveryTypeOption==0)
            {
                ModelState.AddModelError("DeliveryTypeOption", "You Must Select a Delivery Type Option!");
            }
            
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
                    OrderStatusId=newOrderFromUser.OrderStatusId,
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
                return RedirectToAction("IndexBasedOnSts");

            }

            newOrderFromUser.GovList = await govService.GetAllAsync();
            newOrderFromUser.BranchList = await branchService.GetAllAsync();
            newOrderFromUser.ShippingTypeList = await shippingTypeService.GetAllAsync();
            newOrderFromUser.PaymentMethodList = await paymentMethodService.GetAllAsync();
            newOrderFromUser.CityList = await cityService.cityListByGov(newOrderFromUser.GovernorateId);
            newOrderFromUser.OrderStatusList = await orderStatusService.GetAllAsync();

            return View("Add", newOrderFromUser);
        }
       

        //Delete Order From Database
        public async Task<IActionResult> Delete(int Id)
        {
            List<OrderItem> orderItemsList = await orderItemsService.GetAllAsync();
            List<OrderItem> orderItemsRelatedToOrderId =  orderItemsList.Where(o => o.OrderId == Id).ToList();
            foreach(var item in orderItemsRelatedToOrderId)
            {
                await orderItemsService.DeleteAsync(item.Id);
            }
            await orderItemsService.SaveAsync();
            await orderService.DeleteAsync(Id);
            await orderService.SaveAsync();
            return RedirectToAction("Index");
        }

    }
}
