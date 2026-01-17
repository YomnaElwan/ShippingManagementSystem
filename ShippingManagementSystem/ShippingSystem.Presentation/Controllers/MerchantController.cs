using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Presentation.ViewModels.MerchantVM;
using ShippingSystem.Presentation.ViewModels.OrderVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class MerchantController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userService;
        private readonly SignInManager<ApplicationUser> signInManager;
        public MerchantController(IUnitOfWork unitOfWork,
                                  UserManager<ApplicationUser> userService,
                                  SignInManager<ApplicationUser> signInManager
                                 )
        {
            this.unitOfWork = unitOfWork;
            this.userService = userService;
            this.signInManager = signInManager;
        }
        [HttpGet]
        [Authorize(Policy = "ViewMerchantHome")]
        public async Task<IActionResult> MerchantHome()
        {
            List<Orders> orderList = await unitOfWork.OrderRepository.GetAllAsync();

            OrdersHomeVM mappedMerchantHome = new OrdersHomeVM()
            {
                OrderCountByStatus = orderList.GroupBy(o => o.OrderStatusId).ToDictionary(order=>order.Key,order=>order.Count()),
                OrderStatusList = await unitOfWork.OrderStatusRepository.GetAllAsync()

            };
            return View("OrdersHome",mappedMerchantHome);
        }
        [HttpGet]
        [Authorize(Policy = "ViewMerchants")]
        public async Task<IActionResult> Index()
        {
            List<Merchants> merchantList =await unitOfWork.SpecificMerchantRepository.SpecialMerchantsList();
            return View("Index",merchantList);
        }
        [HttpGet]
        [Authorize(Policy = "ViewMerchantDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            Merchants merchantDetails = await unitOfWork.SpecificMerchantRepository.GetMerchantById(Id);
            return View("Details",merchantDetails);
        }
        [HttpGet]
        public async Task<IActionResult> CityList(int govId)
        {
            List<Cities> cityList = await unitOfWork.SpecificCityRepository.cityListByGov(govId);
            return Json(cityList);
        }
        [HttpGet]
        [Authorize(Policy = "AddNewMerchant")]
        public async Task <IActionResult> Add()
        {
            List<Branches> branchList = await unitOfWork.BranchRepository.GetAllAsync();
            List<Governorates> govList = await unitOfWork.GovernorateRepository.GetAllAsync();
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
                    //await signInManager.SignInAsync(newUser, false);
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
                    await unitOfWork.MerchantRepository.AddAsync(newMerchant);
                    await unitOfWork.SaveAsync();
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
            newMerchantFromUser.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
            newMerchantFromUser.CityList = new List<Cities>();
            newMerchantFromUser.GovList = await unitOfWork.GovernorateRepository.GetAllAsync();
            return View("Add", newMerchantFromUser);
        }
        [HttpGet]
        [Authorize(Policy = "EditMerchant")]
        public async Task<IActionResult> Edit(int Id)
        {
            var existMerchant = await unitOfWork.SpecificMerchantRepository.GetMerchantById(Id);
            EditMerchantVM mappedEditMerchant = new EditMerchantVM()
            {
                Id=existMerchant.Id,
                MerchantName=existMerchant.User.UserName,
                MerchantEmail=existMerchant.User.Email,
                MerchantAddress=existMerchant.User.Address,
                MerchantPhoneNumber=existMerchant.User.PhoneNumber,
                BranchId=existMerchant.BranchId,
                CityId=existMerchant.CityId,
                GovernorateId=existMerchant.GovernorateId,
                CompanyName=existMerchant.CompanyName,
                RejOrderCostPercent=existMerchant.RejOrderCostPercent,
                SpecialPackUpCost=existMerchant.SpecialPackUpCost,
            };
            mappedEditMerchant.BranchList =await unitOfWork.BranchRepository.GetAllAsync();
            mappedEditMerchant.CityList = await unitOfWork.SpecificCityRepository.GetAllAsync();
            mappedEditMerchant.GovList = await unitOfWork.GovernorateRepository.GetAllAsync();
            return View("Edit",mappedEditMerchant);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditMerchantVM editedMerchant)
        {
            if (ModelState.IsValid)
            {
                var merchantFromDB = await unitOfWork.SpecificMerchantRepository.GetMerchantById(editedMerchant.Id);
                merchantFromDB.User.UserName = editedMerchant.MerchantName;
                merchantFromDB.User.Email = editedMerchant.MerchantEmail;
                merchantFromDB.User.Address = editedMerchant.MerchantAddress;
                merchantFromDB.User.PhoneNumber = editedMerchant.MerchantPhoneNumber;
                merchantFromDB.BranchId = editedMerchant.BranchId;
                merchantFromDB.CityId =editedMerchant.CityId;
                merchantFromDB.GovernorateId = editedMerchant.GovernorateId;
                merchantFromDB.CompanyName = editedMerchant.CompanyName;
                merchantFromDB.RejOrderCostPercent = editedMerchant.RejOrderCostPercent;
                merchantFromDB.SpecialPackUpCost = editedMerchant.SpecialPackUpCost;
                await unitOfWork.MerchantRepository.UpdateAsync(merchantFromDB);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            editedMerchant.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
            editedMerchant.CityList = new List<Cities>();
            editedMerchant.GovList = await unitOfWork.GovernorateRepository.GetAllAsync();
            return View("Edit",editedMerchant);
        }
        [Authorize(Policy = "DeleteMerchant")]
        public async Task<IActionResult> Delete(int Id)
        {
            await unitOfWork.MerchantRepository.DeleteAsync(Id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }

    }
}
