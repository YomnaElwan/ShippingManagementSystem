using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Presentation.ViewModels.CourierVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class CourierController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public CourierController(IUnitOfWork unitOfWork,
                                 UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
      
        }
        [HttpGet]
        [Authorize(Policy = "ViewCouriers")]
        public async Task <IActionResult> Index()
        {
            List<Couriers> courierList = await unitOfWork.SpecificCourierRepository.CourierList();
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
        [Authorize(Policy= "ViewCourierDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            Couriers courierFromDB = await unitOfWork.SpecificCourierRepository.CourierWithDataById(Id);
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
        [Authorize(Policy = "AddNewCourier")]
        public async Task<IActionResult> Add()
        {
            CourierAddViewModel viewModel = new CourierAddViewModel()
            {
                BranchList = await unitOfWork.BranchRepository.GetAllAsync(),
                GovernoratesList = await unitOfWork.GovernorateRepository.GetAllAsync(),
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
                  //await signInManager.SignInAsync(newUser, false);
                  Couriers newCourier = new Couriers()
                  {
                        BranchId = newCourierFromUser.BranchId,
                        GovernorateId = newCourierFromUser.GovernorateId,
                        User = newUser,
                        DiscountValue = newCourierFromUser.CompanyDiscountValue,
                        DiscountTypeOption = newCourierFromUser.DiscountTypeOptions.Value,
                  };
                    await unitOfWork.CourierRepository.AddAsync(newCourier);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");

                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
             

            }
            newCourierFromUser.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
            newCourierFromUser.GovernoratesList = await unitOfWork.GovernorateRepository.GetAllAsync();
            return View("Add",newCourierFromUser);
        }
        [HttpGet]
        [Authorize(Policy = "EditCourier")]
        public async Task<IActionResult> Edit(int Id)
        {
            Couriers courierFromDB = await unitOfWork.SpecificCourierRepository.CourierWithDataById(Id);
            EditCourierViewModel editCourier = new EditCourierViewModel()
            {
                Id=courierFromDB.Id,
                CourierName = courierFromDB.User.UserName,
                CourierEmail = courierFromDB.User.Email,
                CourierPhone = courierFromDB.User.PhoneNumber,
                CourierAddress = courierFromDB.User.Address,
                BranchId = courierFromDB.BranchId,
                GovernorateId = courierFromDB.GovernorateId,
                DiscountTypeOptions = courierFromDB.DiscountTypeOption,
                CompanyDiscountValue = courierFromDB.DiscountValue
            };
            editCourier.BranchList= await unitOfWork.BranchRepository.GetAllAsync();
            editCourier.GovernorateList = await unitOfWork.GovernorateRepository.GetAllAsync();
            return View("Edit",editCourier);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditCourierViewModel editedModel)
        {
            if (ModelState.IsValid)
            {
                Couriers courierFromDB = await unitOfWork.SpecificCourierRepository.CourierWithDataById(editedModel.Id);
                courierFromDB.User.UserName = editedModel.CourierName;
                courierFromDB.User.Email = editedModel.CourierEmail;
                courierFromDB.User.PhoneNumber = editedModel.CourierPhone;
                courierFromDB.User.Address = editedModel.CourierAddress;
                courierFromDB.BranchId = editedModel.BranchId;
                courierFromDB.GovernorateId = editedModel.GovernorateId;
                courierFromDB.DiscountTypeOption = editedModel.DiscountTypeOptions.Value;
                courierFromDB.DiscountValue = editedModel.CompanyDiscountValue;
                await unitOfWork.CourierRepository.UpdateAsync(courierFromDB);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            editedModel.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
            editedModel.GovernorateList = await unitOfWork.GovernorateRepository.GetAllAsync();
            return View("Edit",editedModel);
        }
        [Authorize(Policy = "DeleteCourier")]
        public async Task<IActionResult> Delete(int Id)
        {
            await unitOfWork.CourierRepository.DeleteAsync(Id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
