using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Mono.TextTemplating;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Application.Services;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Presentation.SessionExtensions;
using ShippingSystem.Presentation.ViewModels.OrderItemsVM;
using ShippingSystem.Presentation.ViewModels.OrderVM;
using System.Security.Claims;
using System.Text.Json;

namespace ShippingSystem.Presentation.Controllers
{
    public class OrderController : Controller
    {
        //Add Cache
        private readonly IDistributedCache cache;
        private readonly IGenericService<Governorates> govService;
        private readonly IGenericService<Branches> branchService;
        private readonly IGenericService<ShippingTypes> shippingTypeService;
        private readonly IGenericService<PaymentMethods> paymentMethodService;
        private readonly ICityService cityService;
        private readonly IGenericService<Orders> orderService;
        private readonly IGenericService<OrderItem> orderItemsService;
        private readonly IMerchantService merchantService;
        private readonly IGenericService<OrderStatus> orderStatusService;
        private readonly IOrderService customOrderService;
        private readonly IWeightSettingsService weightSettService;
        private readonly IOrderItemService specialOrderItemsService;
        public OrderController(IGenericService<Governorates> govService,
                               IGenericService<Branches> branchService,
                               IGenericService<ShippingTypes> shippingTypeService,
                               IGenericService<PaymentMethods> paymentMethodService,
                               ICityService cityService,
                               IGenericService<Orders> orderService,
                               IGenericService<OrderItem> orderItemsService,
                               IMerchantService merchantService,
                               IGenericService<OrderStatus> orderStatusService,
                               IOrderService customOrderService,
                               IWeightSettingsService weightSettService,
                               IOrderItemService specialOrderItemsService,
                               IDistributedCache cache
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
            this.weightSettService = weightSettService;
            this.specialOrderItemsService = specialOrderItemsService;
            this.cache = cache;
        }
        //Get Merchant and Employee Home Page
        [HttpGet]
        public async Task<IActionResult> OrdersHome()
        {
            List<Orders> orderList = await orderService.GetAllAsync();
            OrdersHomeVM mappedMerchantHome = new OrdersHomeVM()
            {
                OrderCountByStatus = orderList.GroupBy(o => o.OrderStatusId).ToDictionary(order => order.Key, order => order.Count()),
                OrderStatusList = await orderStatusService.GetAllAsync()
            };
            return View("OrdersHome", mappedMerchantHome);
        }
        //Get Orders List
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var merchant = (await merchantService.SpecialMerchantsList()).FirstOrDefault(m => m.UserId == userId);
            List<Orders> orderList = await customOrderService.GetSpecialOrderList();
            List<GetOrdersVM> mappedOrderList = orderList.Select(order => new GetOrdersVM
            {
                OrderId = order.Id,
                StatusName = order.OrderStatus?.Name??"N/A",
                CustomerName = order?.CustomerName??"N/A",
                CustomerPhoneNum = order?.PhoneNumber1??"N/A",
                GovName = order?.Governorate?.Name??"N/A",
                CityName = order?.City?.Name??"N/A",
                OrderTotalCost = order?.TotalCost??0,
                CreateAt = order?.CreateAt??DateTime.MinValue,
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
                CustomerPhoneNum=order.PhoneNumber1,
                GovName=order.Governorate?.Name,
                CityName=order.City?.Name,
                OrderTotalCost=order.TotalCost,
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
            // Get OrderItems From Session or Create new one
            List<GetOrderItemsVM> items = HttpContext.Session.GetObjectFromJson<List<GetOrderItemsVM>>("OrderItems")
                                         ?? new List<GetOrderItemsVM>();

            // Add New Product
            items.Add(new GetOrderItemsVM
            {
                Id = items.Count + 1,
                ProductName = product.ProductName,
                Quantity = product.Quantity,
                Weight = product.Weight,
                Price = product.Price
            });

            // Update Session
            HttpContext.Session.SetObjectAsJson("OrderItems", items);

            // return Partial View With OrderItems
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
                MerchantPhoneNum = merchant?.User?.PhoneNumber ?? "null",
                MerchantAddress = merchant?.User?.Address ?? "null",
            };
            return View("Add",addOrderVM);
        }
        //Get City Costs & WeightSettings Based On City Id
        [HttpGet]
        public async Task<IActionResult> GetCityCostsAndWeightSettById(int cityId)
        {
            Cities cityById = await cityService.GetByIdAsync(cityId);
            WeightSettings weightSett = await weightSettService.GetWeightSettByCityId(cityId);

            return Json(new
            {
                deliveryCost=cityById?.DeliveryCost??0,
                pickupCost=cityById?.PickupCost??0,
                baseWeightLimit = weightSett?.BaseWeightLimit??0,
                priceForExtraKGs = weightSett?.PricePerKg??0,

            });
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
                decimal totalShippingCost=0;

                Cities selectedCity = await cityService.GetByIdAsync(newOrderFromUser.CityId);
                if (selectedCity != null)
                {
                    if (newOrderFromUser.DeliveryTypeOption == DeliveryMethod.HomeDelivery)
                    {
                        totalCost += selectedCity.DeliveryCost;
                        totalShippingCost = selectedCity.DeliveryCost;
                    }
                    else if (newOrderFromUser.DeliveryTypeOption==DeliveryMethod.BranchPickup)
                    {
                        totalCost += selectedCity.PickupCost;
                        totalShippingCost = selectedCity.PickupCost;
                        
                    }
                }
                WeightSettings weightSettingsByCityId = await weightSettService.GetWeightSettByCityId(newOrderFromUser.CityId);
                if (weightSettService != null)
                {
                    if (totalWeight > weightSettingsByCityId.BaseWeightLimit)
                    {
                        var extraCost = (totalWeight - newOrderFromUser.BaseWeightLimit) * weightSettingsByCityId.PricePerKg;
                        totalCost += extraCost;
                        totalShippingCost += extraCost;
                    }
                }

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
                    TotalWeight = totalWeight,
                    MerchantId = merchant.Id,
                    Notes = newOrderFromUser.Notes,
                    OrderStatusId=newOrderFromUser.OrderStatusId,
                    ReceivedAmount=newOrderFromUser.ReceivedAmount,
                    ReceivedDeliveryCost=newOrderFromUser.ReceivedDeliveryCost,
                    ShippingTotalCost=totalShippingCost,
                };

                await orderService.AddAsync(newOrder);
                await orderService.SaveAsync();

                //Get OrderItems From Session
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

                // After Saving OrderItems To Database, Remove Session
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
        //Edit Order
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var merchant = (await merchantService.SpecialMerchantsList()).FirstOrDefault(m => m.UserId == userId);
            
            Orders orderFromDB = await customOrderService.GetByIdAsync(Id);
            if (orderFromDB == null)
            {
                return NotFound();
            }
            Cities cityById = await cityService.GetByIdAsync(orderFromDB.CityId);
            WeightSettings weightSettByCityId = await weightSettService.GetWeightSettByCityId(orderFromDB.CityId);
            List<OrderItem> orderItemsByOrderId = await specialOrderItemsService.GetOrderItemsByOrderId(orderFromDB.Id);
            List<GetOrderItemsVM> mappedOrderItems = orderItemsByOrderId.Select(orderItem => new GetOrderItemsVM
            {
               Id=orderItem.Id,
               Price=orderItem.Price,
               Quantity=orderItem.Quantity,
               ProductName=orderItem.ProductName,
               Weight=orderItem.Weight
            }).ToList();
            HttpContext.Session.SetObjectAsJson("OrderItems",mappedOrderItems);
            EditOrderVM mappedEditOrder = new EditOrderVM()
            {
                Id = orderFromDB.Id,
                CustomerName = orderFromDB.CustomerName,
                PhoneNumber1 = orderFromDB.PhoneNumber1,
                PhoneNumber2 = orderFromDB.PhoneNumber2,
                CustomerEmail = orderFromDB.CustomerEmail,
                Address = orderFromDB.Address,
                Notes = orderFromDB.Notes,
                CreateAt=orderFromDB.CreateAt,
                VillageDelivery = orderFromDB.VillageDelivery,
                GovernorateId = orderFromDB.GovernorateId,
                CityId = orderFromDB.CityId,
                BranchId = orderFromDB.BranchId,
                ShippingTypeId = orderFromDB.ShippingTypeId,
                PaymentMethodId = orderFromDB.PaymentMethodId,
                DeliveryTypeOption = orderFromDB.DeliveryTypeOption,
                OrderStatusId = orderFromDB.OrderStatusId,
                MerchantAddress = merchant?.User.Address ?? "null",
                MerchantPhoneNum = merchant?.User.PhoneNumber ?? "null",
                BranchList = await branchService.GetAllAsync(),
                GovList = await govService.GetAllAsync(),
                CityList = await cityService.cityListByGov(orderFromDB.GovernorateId),
                ShippingTypeList = await shippingTypeService.GetAllAsync(),
                PaymentMethodList=await paymentMethodService.GetAllAsync(),
                OrderStatusList= await orderStatusService.GetAllAsync(),
                DeliveryCost=cityById.DeliveryCost,
                PickupCost=cityById.PickupCost,
                BaseWeightLimit=weightSettByCityId?.BaseWeightLimit??0,
                PricePerKg=weightSettByCityId?.PricePerKg??0,
                OrderItems=mappedOrderItems,
                ReceivedAmount=orderFromDB.ReceivedAmount,
                ReceivedDeliveryCost=orderFromDB.ReceivedDeliveryCost,
            };
            return View("Edit",mappedEditOrder);
        }
        [HttpPost]
        public async Task<IActionResult> SaveEdit(EditOrderVM editOrderFromUser) {
            List<GetOrderItemsVM> orderItemsFromSession = HttpContext.Session.GetObjectFromJson<List<GetOrderItemsVM>>("OrderItems");
            decimal totalCost = orderItemsFromSession.Sum(item => item.Quantity * item.Price);
            decimal totalWeight = orderItemsFromSession.Sum(item => item.Quantity * item.Weight);
            decimal totalShippingCost = 0;

            Cities selectedCity =  await cityService.GetByIdAsync(editOrderFromUser.CityId);
            if (selectedCity != null)
            {
                if (editOrderFromUser.DeliveryTypeOption == DeliveryMethod.HomeDelivery)
                {
                    totalCost += selectedCity.DeliveryCost;
                    totalShippingCost = selectedCity.DeliveryCost;
                }
                else if (editOrderFromUser.DeliveryTypeOption == DeliveryMethod.BranchPickup)
                {
                    totalCost += selectedCity.PickupCost;
                    totalShippingCost = selectedCity.PickupCost;
                }
            }
            WeightSettings weightSettingsByCityId = await weightSettService.GetWeightSettByCityId(editOrderFromUser.CityId);
            if (totalWeight > weightSettingsByCityId.BaseWeightLimit)
            {
                decimal extraWeight = totalWeight-weightSettingsByCityId.BaseWeightLimit;
                decimal extraPrice = extraWeight * weightSettingsByCityId.PricePerKg;
                totalCost += extraPrice;
                totalShippingCost += extraPrice;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var merchant = (await merchantService.GetAllAsync()).FirstOrDefault(m => m.UserId == userId);
            if (editOrderFromUser.CityId == 0)
            {
                ModelState.AddModelError("CityId", "Select a City!");
            }
            Orders orderFromDB = await customOrderService.GetByIdAsync(editOrderFromUser.Id);
            if (ModelState.IsValid)
            {
                orderFromDB.Id = editOrderFromUser.Id;
                orderFromDB.CustomerName = editOrderFromUser.CustomerName;
                orderFromDB.PhoneNumber1 = editOrderFromUser.PhoneNumber1;
                orderFromDB.PhoneNumber2 = editOrderFromUser.PhoneNumber2;
                orderFromDB.CustomerEmail = editOrderFromUser.CustomerEmail;
                orderFromDB.Address = editOrderFromUser.Address;
                orderFromDB.Notes = editOrderFromUser.Notes;
                orderFromDB.VillageDelivery = editOrderFromUser.VillageDelivery;
                orderFromDB.GovernorateId = editOrderFromUser.GovernorateId;
                orderFromDB.BranchId = editOrderFromUser.BranchId;
                orderFromDB.DeliveryTypeOption = editOrderFromUser.DeliveryTypeOption;
                orderFromDB.ShippingTypeId = editOrderFromUser.ShippingTypeId;
                orderFromDB.PaymentMethodId = editOrderFromUser.PaymentMethodId;
                orderFromDB.CityId = editOrderFromUser.CityId;
                orderFromDB.CreateAt = editOrderFromUser.CreateAt;
                orderFromDB.OrderStatusId = editOrderFromUser.OrderStatusId;
                orderFromDB.TotalCost = totalCost;
                orderFromDB.TotalWeight = totalWeight;
                orderFromDB.MerchantId = merchant.Id;
                orderFromDB.ReceivedAmount = editOrderFromUser.ReceivedAmount;
                orderFromDB.ReceivedDeliveryCost = editOrderFromUser.ReceivedDeliveryCost;
                orderFromDB.ShippingTotalCost = totalShippingCost;
                await orderService.UpdateAsync(orderFromDB);
                await orderService.SaveAsync();
                
                List<OrderItem> existingOrderItems = await specialOrderItemsService.GetOrderItemsByOrderId(editOrderFromUser.Id);
                foreach(var oldItem in existingOrderItems)
                {
                    await orderItemsService.DeleteAsync(oldItem.Id);
                }
                await orderItemsService.SaveAsync();
                if(orderItemsFromSession!=null && orderItemsFromSession.Any())
                {
                    foreach(var item in orderItemsFromSession)
                    {
                        OrderItem orderItem = new OrderItem() {
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            Weight = item.Weight,
                            OrderId = editOrderFromUser.Id,
                            Price = item.Price,
                        };
                        await orderItemsService.AddAsync(orderItem);
                    }
                    await orderItemsService.SaveAsync();
                }
                HttpContext.Session.Remove("OrderItems");
                return RedirectToAction("IndexBasedOnSts");
            }
        
            return View("Edit", editOrderFromUser);
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
            return RedirectToAction("IndexBasedOnSts");
        }
 
        
        //Delete From Order Items Table While Adding New Order
        public IActionResult DeleteFromOrderItems(int Id) {
            List<GetOrderItemsVM> items = HttpContext.Session.GetObjectFromJson<List<GetOrderItemsVM>>("OrderItems") ?? new List<GetOrderItemsVM>();

            var itemToRemove = items.FirstOrDefault(item => item.Id == Id);
            if (itemToRemove!=null)
            {
                items.Remove(itemToRemove);
            }
            HttpContext.Session.SetObjectAsJson("OrderItems", items);
            return PartialView("_GetOrderItemsPartial", items);
        }
        //Edit OrderItems
        [HttpPost]
        public IActionResult SaveEditOrderItems([FromBody] GetOrderItemsVM updatedOrderItems)
        {
            if (updatedOrderItems == null)
            {
                return BadRequest("Invalid Updated Order Item");
            }
            List<GetOrderItemsVM> getOrderItemsFromSession = HttpContext.Session.GetObjectFromJson<List<GetOrderItemsVM>>("OrderItems");
            if (getOrderItemsFromSession == null)
                getOrderItemsFromSession = new List<GetOrderItemsVM>();

            var existingOrderItem = getOrderItemsFromSession.FirstOrDefault(item => item.Id == updatedOrderItems.Id);
            if (existingOrderItem != null)
            {
                existingOrderItem.ProductName = updatedOrderItems.ProductName;
                existingOrderItem.Weight = updatedOrderItems.Weight;
                existingOrderItem.Price = updatedOrderItems.Price;
                existingOrderItem.Quantity = updatedOrderItems.Quantity;
            }
            HttpContext.Session.SetObjectAsJson("OrderItems", getOrderItemsFromSession);
            return Ok();
        }
        //Order Report
        [HttpGet]
        public async Task<IActionResult> OrderReport()
        {
            List<OrderStatus> orderStatusList = await orderStatusService.GetAllAsync();
            return View("OrderReport",orderStatusList);
        }
        [HttpGet]
        public async Task<IActionResult> OrderReportForAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var merchant = (await merchantService.SpecialMerchantsList()).FirstOrDefault(m => m.UserId == userId);
            var cacheKey = "OrderReportForAll";
            var cachedData = await cache.GetStringAsync(cacheKey);
            List<GetOrdersVM> orderReportData;
            if (cachedData != null)
            {
                orderReportData = JsonSerializer.Deserialize<List<GetOrdersVM>>(cachedData);
            }
            else
            {
                List<Orders> GetOrdersFromDB = await customOrderService.GetSpecialOrderList();
                orderReportData = GetOrdersFromDB.Select(order => new GetOrdersVM
                {
                    OrderId=order.Id,
                    StatusName=order?.OrderStatus?.Name ?? "N/A",
                    MerchantName=merchant?.User?.UserName??"N/A",
                    CustomerName=order?.CustomerName??"N.A",
                    CustomerPhoneNum=order?.PhoneNumber1??"N/A",
                    GovName=order?.Governorate?.Name??"N/A",
                    CityName=order?.City?.Name??"N/A",
                    OrderTotalCost=order?.TotalCost??0,
                    ReceivedAmount=order?.ReceivedAmount??0,
                    ShippingTotalCost=order?.ShippingTotalCost??0,
                    ReceivedDeliveryCost=order?.ReceivedDeliveryCost??0,
                    CompanyPercent=order?.Courier?.DiscountValue??0,
                    CreateAt=order?.CreateAt??DateTime.MinValue,
                    OrderStatusId=order?.OrderStatusId??0,
                }).ToList();
                await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(orderReportData),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
            }

            return PartialView("_OrderReportForAll",orderReportData);
        }
        [HttpGet]
        public async Task<IActionResult> GetOrdersByStsId(int orderStsId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var merchant = (await merchantService.SpecialMerchantsList()).FirstOrDefault(merchant => merchant.UserId == userId);
            List<GetOrdersVM> orderReportData;
            var cacheKey = $"OrdersByStatus_{orderStsId}";
            var cachedData = await cache.GetStringAsync(cacheKey);
            List<Orders> ordersFromDBByOrderSts = await customOrderService.GetOrdersByOrderStsId(orderStsId);
            if (cachedData != null)
            {
                orderReportData = JsonSerializer.Deserialize<List<GetOrdersVM>>(cachedData);
            }
            else
            {
                orderReportData = ordersFromDBByOrderSts.Select(o => new GetOrdersVM
                {
                    OrderId=o.Id,
                    StatusName=o?.OrderStatus?.Name??"N/A",
                    MerchantName=merchant?.User?.UserName??"N/A",
                    CustomerName=o?.CustomerName??"N/A",
                    CustomerPhoneNum=o?.PhoneNumber1??"N/A",
                    GovName=o?.Governorate?.Name??"N/A",
                    CityName=o?.City?.Name??"N/A",
                    OrderTotalCost=o?.TotalCost??0,
                    ReceivedAmount=o?.ReceivedAmount??0,
                    ShippingTotalCost=o?.ShippingTotalCost??0,
                    ReceivedDeliveryCost=o?.ReceivedDeliveryCost??0,
                    CompanyPercent=o?.Courier?.DiscountValue??0,
                    CreateAt=o?.CreateAt??DateTime.MinValue,
                    OrderStatusId=o?.OrderStatusId??0,
                }).ToList();
                await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(orderReportData),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
            }
            return PartialView("_OrderReportForAll",orderReportData);
            
        }
    }

}
