using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Application.Services;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Presentation.ViewModels.OrderItemsVM;
using ShippingSystem.Presentation.ViewModels.OrderVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class OrderController : Controller
    {
        private readonly IGenericService<Governorates> govService;
        private readonly IGenericService<Branches> branchService;
        private readonly IGenericService<ShippingType> shippingTypeService;
        private readonly IGenericService<PaymentMethod> paymentMethodService;
        private readonly ICityService _cityService;
        private readonly IGenericService<OrderItem> orderItemsService;
        public OrderController(IGenericService<Governorates> govService,
                               IGenericService<Branches> branchService,
                               IGenericService<ShippingType> shippingTypeService,
                               IGenericService<PaymentMethod> paymentMethodService,
                               ICityService _cityService,
                               IGenericService<OrderItem> orderItemsService)
        {
            this.govService = govService;
            this.branchService = branchService;
            this.shippingTypeService = shippingTypeService;
            this.paymentMethodService = paymentMethodService;
            this._cityService = _cityService;
            this.orderItemsService = orderItemsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCityList(int govId)
        {
            List<Cities> cityList = await _cityService.cityListByGov(govId);
            return Json(cityList);
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            List<Governorates> govList = await govService.GetAllAsync();
            List<Branches> branchList = await branchService.GetAllAsync();
            List<ShippingType> shippingTypeList = await shippingTypeService.GetAllAsync();
            List<PaymentMethod> paymentMethodList = await paymentMethodService.GetAllAsync();

            // هات المنتجات (OrderItems) من الداتابيز
            // أو لو لسه بتضيفهم مؤقت ممكن ترجع لستة فاضية
            var orderItems = await orderItemsService.GetAllAsync();
            var orderItemsVM = orderItems.Select(o => new GetOrderItemsVM
            {
                Id = o.Id,
                ProductName = o.ProductName,
                Quantity = o.Quantity,
                Weight = o.Weight,
                Price = o.Price
            }).ToList();

            AddOrderVM orderViewModel = new AddOrderVM()
            {
                GovList = govList,
                BranchList = branchList,
                ShippingTypeList = shippingTypeList,
                PaymentMethodList = paymentMethodList,
                CityList = new List<Cities>(),
                OrderItems = orderItemsVM
            };

            return View("Add", orderViewModel);
        }

        //public async Task<IActionResult> Add()
        //{
        //    List<Governorates> govList = await govService.GetAllAsync();
        //    List<Branches> branchList = await branchService.GetAllAsync();
        //    List<ShippingType> shippingTypeList = await shippingTypeService.GetAllAsync();
        //    List<PaymentMethod> paymentMethodList = await paymentMethodService.GetAllAsync();

        //    AddOrderVM orderViewModel = new AddOrderVM()
        //    {
        //        GovList = govList,
        //        BranchList = branchList,
        //        ShippingTypeList = shippingTypeList,
        //        PaymentMethodList = paymentMethodList,
        //        CityList = new List<Cities>(),
        //        OrderItems = new List<GetOrderItemsVM>()
        //    };

        //    return View("Add",orderViewModel);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAdd(AddOrderVM newOrder)
        {
           
            return View("Add", newOrder);
        }
    }
}
