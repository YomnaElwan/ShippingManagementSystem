using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels.MerchantVM;
using System.Linq.Expressions;

namespace ShippingSystem.Presentation.Controllers
{
    public class MerchantController : Controller
    {
        private readonly IGenericService<Governorates> govService;
        private readonly IGenericService<Branches> branchService;
        private readonly ICityService cityService;
        private readonly UserManager<ApplicationUser> userService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IGenericService<Merchants> merchantService;
        private readonly IMerchantService _merService;
        public MerchantController(IGenericService<Governorates> govService,
                                  IGenericService<Branches> branchService,
                                  ICityService cityService,
                                  UserManager<ApplicationUser> userService,
                                  SignInManager<ApplicationUser> signInManager,
                                  IGenericService<Merchants> merchantService,
                                  IMerchantService _merService)
        {
            this.govService = govService;
            this.branchService = branchService;
            this.cityService = cityService;
            this.userService = userService;
            this.signInManager = signInManager;
            this.merchantService = merchantService;
            this._merService = _merService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Merchants> merchantList =await _merService.SpecialMerchantsList();
            return View("Index",merchantList);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            return View("Details");
        }
        [HttpGet]
        public async Task<IActionResult> CityList(int govId)
        {
            List<Cities> cityList = await cityService.cityListByGov(govId);
            return Json(cityList);
        }
        [HttpGet]
        public async Task <IActionResult> Add()
        {
            List<Branches> branchList = await branchService.GetAllAsync();
            List<Governorates> govList = await govService.GetAllAsync();
            AddMerchantVM newMerchant = new AddMerchantVM()
            {
                BranchList = branchList,
                GovList = govList,
                CityList = new List<Cities>()

            };
            return View("Add",newMerchant);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> SaveAdd(AddMerchantVM newMerchantFromUser)
        {
            if (newMerchantFromUser.BranchId == 0)
            {
                ModelState.AddModelError("BranchId", "You must select a branch!");
            }
            if (newMerchantFromUser.CityId == 0)
            {
                ModelState.AddModelError("CityId", "You must select a city");
            }
            if (newMerchantFromUser.GovernorateId == 0)
            {
                ModelState.AddModelError("GovernorateId", "You must select a governorate!");
            }
            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new ApplicationUser() {
                    UserName=newMerchantFromUser.MerchantName,
                    Email=newMerchantFromUser.MerchantEmail,
                    PhoneNumber=newMerchantFromUser.MerchantPhoneNumber,
                    Address=newMerchantFromUser.MerchantAddress,
                };
                IdentityResult result = await userService.CreateAsync(newUser, newMerchantFromUser.MerchantPassword);
                if (result.Succeeded)
                {
                    await userService.AddToRoleAsync(newUser,"Merchant");
                    await signInManager.SignInAsync(newUser, false);
                    Merchants newMerchant = new Merchants()
                    {
                        User = newUser,
                        CompanyName = newMerchantFromUser.CompanyName,
                        RejOrderCostPercent = newMerchantFromUser.RejOrderCostPercent,
                        SpecialPackUpCost = newMerchantFromUser.SpecialPackUpCost,
                        BranchId = newMerchantFromUser.BranchId,
                        CityId = newMerchantFromUser.CityId,
                        GovernorateId = newMerchantFromUser.GovernorateId
                    };
                    await merchantService.AddAsync(newMerchant);
                    await merchantService.SaveAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach(var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            newMerchantFromUser.BranchList = await branchService.GetAllAsync();
            newMerchantFromUser.CityList = new List<Cities>();
            newMerchantFromUser.GovList = await govService.GetAllAsync();
            return View("Add", newMerchantFromUser);
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
            await merchantService.DeleteAsync(Id);
            await merchantService.SaveAsync();
            return RedirectToAction("Index");
        }

    }
}
