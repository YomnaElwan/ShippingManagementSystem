using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels.CourierVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class CourierController : Controller
    {
        private readonly IGenericService<Governorates> govService;
        private readonly IGenericService<Branches> branchService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IGenericService<Couriers> courierService;
        private readonly ICourierService serviceCourier;
        public CourierController(IGenericService<Governorates> govService,
                                 IGenericService<Branches> branchService,
                                 UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IGenericService<Couriers> courierService,
                                 ICourierService serviceCourier)
        {
            this.govService = govService;
            this.branchService = branchService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.courierService = courierService;
            this.serviceCourier = serviceCourier;
        }
        [HttpGet]
        public async Task <IActionResult> Index()
        {
            List<Couriers> courierList = await serviceCourier.CourierList();
            List<ReadCourierViewModel> courierListMapped = courierList.Select(c => new ReadCourierViewModel
            {
                CourierId = c.Id,
                BranchName = c.Branch.Name,
                CourierName = c.User.UserName,
                CourierEmail = c.User.Email,
                CourierPhone = c.User.PhoneNumber,
                IsActive=c.IsActive

            }).ToList();
            return View("Index",courierListMapped);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            Couriers courierFromDB = await serviceCourier.CourierWithDataById(Id);
            ReadCourierViewModel mappedCourier = new ReadCourierViewModel()
            {
                CourierId = courierFromDB.Id,
                CourierName = courierFromDB.User.UserName,
                CourierEmail = courierFromDB.User.Email,
                CourierPhone = courierFromDB.User.PhoneNumber,
                CourierAddress = courierFromDB.User.Address,
                BranchName = courierFromDB.Branch.Name,
                GovernorateName = courierFromDB.Governorate.Name,
                DiscountTypeOptions = courierFromDB.DiscountTypeOption,
                CompanyDiscountValue = courierFromDB.DiscountValue
            };

            return View("Details",mappedCourier);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            CourierAddViewModel viewModel = new CourierAddViewModel()
            {
                BranchList = await branchService.GetAllAsync(),
                GovernoratesList = await govService.GetAllAsync(),
            };
            return View("Add",viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNew(CourierAddViewModel newCourierFromUser)
        {
            if (newCourierFromUser.BranchId==0)
            {
                ModelState.AddModelError("BranchId", "You Must Select a Branch");
            }
            if (newCourierFromUser.GovernorateId == 0)
            {
                ModelState.AddModelError("GovernorateId", "You Must Select a Governorate");
            }
            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new ApplicationUser()
                {
                    UserName=newCourierFromUser.CourierName,
                    Email=newCourierFromUser.CourierEmail,
                    Address=newCourierFromUser.CourierAddress,
                    PhoneNumber=newCourierFromUser.CourierPhone,
                };
                IdentityResult result = await userManager.CreateAsync(newUser, newCourierFromUser.CourierPassword);
                if (result.Succeeded)
                {
                  await userManager.AddToRoleAsync(newUser, "Courier");
                  await signInManager.SignInAsync(newUser, false);
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                Couriers newCourier = new Couriers() {
                    BranchId=newCourierFromUser.BranchId,
                    GovernorateId=newCourierFromUser.GovernorateId,
                    User=newUser,
                    DiscountValue=newCourierFromUser.CompanyDiscountValue,
                    DiscountTypeOption=newCourierFromUser.DiscountTypeOptions.Value,
                };
                await courierService.AddAsync(newCourier);
                await courierService.SaveAsync();
                return RedirectToAction("Index");

            }
            newCourierFromUser.BranchList = await branchService.GetAllAsync();
            newCourierFromUser.GovernoratesList = await govService.GetAllAsync();
            return View("Add",newCourierFromUser);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            Couriers courierFromDB = await serviceCourier.CourierWithDataById(Id);
            EditCourierViewModel editedCourier = new EditCourierViewModel()
            {
                CourierName=courierFromDB.User.UserName,
                CourierEmail=courierFromDB.User.Email,
                CourierPhone=courierFromDB.User.PhoneNumber,
                CourierAddress=courierFromDB.User.Address,
                BranchId=courierFromDB.BranchId,
                GovernorateId=courierFromDB.GovernorateId,
                DiscountTypeOptions=courierFromDB.DiscountTypeOption,
                CompanyDiscountValue=courierFromDB.DiscountValue,
            };
            editedCourier.BranchList = await branchService.GetAllAsync();
            editedCourier.GovernorateList = await govService.GetAllAsync();
            return View("Edit",editedCourier);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditCourierViewModel editFromUser)
        {
            if (ModelState.IsValid)
            {
                Couriers editedCourier = await serviceCourier.CourierWithDataById(editFromUser.CourierId);
                editedCourier.Id = editFromUser.CourierId;
                editedCourier.User.UserName = editFromUser.CourierName;
                editedCourier.User.Email = editFromUser.CourierEmail;
                editedCourier.User.PhoneNumber = editFromUser.CourierPhone;
                editedCourier.User.Address = editFromUser.CourierAddress;
                editedCourier.BranchId = editFromUser.BranchId;
                editedCourier.GovernorateId = editFromUser.GovernorateId;
                editedCourier.DiscountTypeOption = editFromUser.DiscountTypeOptions.Value;
                editedCourier.DiscountValue = editFromUser.CompanyDiscountValue;
                
                await courierService.UpdateAsync(editedCourier);
                await courierService.SaveAsync();
                return View("Index");
            }
            editFromUser.BranchList = await branchService.GetAllAsync();
            editFromUser.GovernorateList = await govService.GetAllAsync();
            return View("Edit",editFromUser);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            await courierService.DeleteAsync(Id);
            await courierService.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
