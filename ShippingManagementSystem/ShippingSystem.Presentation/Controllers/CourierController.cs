using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Common.Exceptions;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Application.Services;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Presentation.ViewModels.CourierVM;
using System.IO.Compression;

namespace ShippingSystem.Presentation.Controllers
{
    public class CourierController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        public CourierController(IUnitOfWork unitOfWork,
                                 UserManager<ApplicationUser> userManager,
                                 IMapper mapper
                                 )
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Policy = "ViewCouriers")]
        public async Task <IActionResult> Index()
        {
            List<Couriers> courierList = await unitOfWork.SpecificCourierRepository.CourierList();
            #region manual mapping
            //List<ReadCourierViewModel> courierListMapped = courierList.Select(c => new ReadCourierViewModel
            //{
            //    CourierId = c.Id,
            //    BranchName = c.Branch.Name,
            //    CourierName = c.User.UserName,
            //    CourierEmail = c.User.Email,
            //    CourierPhone = c.User.PhoneNumber,
            //    IsActive=c.IsActive

            //}).ToList();
            //return View("Index",courierListMapped);
            #endregion
            #region auto mapper 
            List<ReadCourierViewModel> readCourierVM = mapper.Map<List<ReadCourierViewModel>>(courierList);
            return View("Index", readCourierVM);
            #endregion

        }
        [HttpGet]
        [Authorize(Policy= "ViewCourierDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            Couriers courierFromDB = await unitOfWork.SpecificCourierRepository.CourierWithDataById(Id);
            if (courierFromDB == null)
                return NotFound($"Courier with id :{Id} isn't found!");
            #region Manual Mapping
            //ReadCourierViewModel mappedCourier = new ReadCourierViewModel()
            //{
            //    CourierId = courierFromDB.Id,
            //    CourierName = courierFromDB.User.UserName,
            //    CourierEmail = courierFromDB.User.Email,
            //    CourierPhone = courierFromDB.User.PhoneNumber,
            //    CourierAddress = courierFromDB.User.Address,
            //    BranchName = courierFromDB.Branch.Name,
            //    GovernorateName = courierFromDB.Governorate.Name,
            //    DiscountTypeOptions = courierFromDB.DiscountTypeOption,
            //    CompanyDiscountValue = courierFromDB.DiscountValue
            //};

            //return View("Details",mappedCourier);
            #endregion
            #region automapper
            ReadCourierViewModel mappedReadCourier = mapper.Map<ReadCourierViewModel>(courierFromDB);
            return View("Details", mappedReadCourier);
            #endregion
        }
        [HttpGet]
        [Authorize(Policy = "AddNewCourier")]
        public async Task<IActionResult> Add()
        {
            AddCourierViewModel viewModel = new AddCourierViewModel()
            {
                BranchList = await unitOfWork.BranchRepository.GetAllAsync(),
                GovernoratesList = await unitOfWork.GovernorateRepository.GetAllAsync(),
            };
            return View("Add",viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SaveNew(AddCourierViewModel newCourierFromUser)
        {
            if (newCourierFromUser.BranchId == 0)
            {
                ModelState.AddModelError("BranchId", "You Must Select a Branch");
            }
            if (newCourierFromUser.GovernorateId == 0)
            {
                ModelState.AddModelError("GovernorateId", "You Must Select a Governorate");
            }

            if (ModelState.IsValid) {
                #region Manual Mapping For New User
                //ApplicationUser newUser = new ApplicationUser()
                //{
                //    UserName = newCourierFromUser.CourierName,
                //    Email = newCourierFromUser.CourierEmail,
                //    Address = newCourierFromUser.CourierAddress,
                //    PhoneNumber = newCourierFromUser.CourierPhone,
                //};
                #endregion
                ApplicationUser newUser = mapper.Map<ApplicationUser>(newCourierFromUser);
                IdentityResult result = await userManager.CreateAsync(newUser, newCourierFromUser.CourierPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "Courier");
                    //await signInManager.SignInAsync(newUser, false);
                    #region Manual Mapping For New Courier
                    //Couriers newCourier = new Couriers()
                    //{
                    //    BranchId = newCourierFromUser.BranchId,
                    //    GovernorateId = newCourierFromUser.GovernorateId,
                    //    User = newUser,
                    //    DiscountValue = newCourierFromUser.CompanyDiscountValue,
                    //    DiscountTypeOption = newCourierFromUser.DiscountTypeOptions.Value,
                    //};
                    #endregion

                    Couriers newCourier = mapper.Map<Couriers>(newCourierFromUser);
                    newCourier.User = newUser;

                    await unitOfWork.CourierRepository.AddAsync(newCourier);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
               
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }

            }
            newCourierFromUser.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
            newCourierFromUser.GovernoratesList = await unitOfWork.GovernorateRepository.GetAllAsync();
            return View("Add", newCourierFromUser);

        }

        [HttpGet]
        [Authorize(Policy = "EditCourier")]
        public async Task<IActionResult> Edit(int Id)
        {
            Couriers courierFromDB = await unitOfWork.SpecificCourierRepository.CourierWithDataById(Id);
            if (courierFromDB == null)
                return NotFound($"Courier with id={Id} can't be found!");
            #region  Manual Mapping
            //EditCourierViewModel editCourier = new EditCourierViewModel()
            //{
            //    Id = courierFromDB.Id,
            //    CourierName = courierFromDB.User.UserName,
            //    CourierEmail = courierFromDB.User.Email,
            //    CourierPhone = courierFromDB.User.PhoneNumber,
            //    CourierAddress = courierFromDB.User.Address,
            //    BranchId = courierFromDB.BranchId,
            //    GovernorateId = courierFromDB.GovernorateId,
            //    DiscountTypeOptions = courierFromDB.DiscountTypeOption,
            //    CompanyDiscountValue = courierFromDB.DiscountValue
            //};
            #endregion
            #region Auto Mapper
            EditCourierViewModel editCourier = mapper.Map<EditCourierViewModel>(courierFromDB);
            #endregion
            editCourier.BranchList= await unitOfWork.BranchRepository.GetAllAsync();
            editCourier.GovernorateList = await unitOfWork.GovernorateRepository.GetAllAsync();
            return View("Edit",editCourier);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditCourierViewModel editedModel)
        {
            if (!ModelState.IsValid)
            {
                editedModel.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
                editedModel.GovernorateList = await unitOfWork.GovernorateRepository.GetAllAsync();
                return View("Edit", editedModel);
            }
            try
            {
                var courierFromDB = await unitOfWork.SpecificCourierRepository.CourierWithDataById(editedModel.Id);
                var getCourierByEmail = await userManager.FindByEmailAsync(editedModel.CourierEmail);
                var getCourierByName = await userManager.FindByNameAsync(editedModel.CourierName);
                if (getCourierByEmail!=null && courierFromDB.UserId!=getCourierByEmail.Id)
                {
                    throw new DuplicateEntityException("Courier", "Email",editedModel.CourierEmail);
                    //throw new Exception("This email is already existing !");
                }
                if(getCourierByName!=null && courierFromDB.UserId!= getCourierByName.Id)
                {
                    throw new DuplicateEntityException("Courier", "Name", editedModel.CourierName);
                    //throw new Exception("This name is already existing");
                }
                mapper.Map(editedModel,courierFromDB.User);
                var result = await userManager.UpdateAsync(courierFromDB.User);
                if (result.Succeeded)
                {
                    mapper.Map(editedModel, courierFromDB);
                    await unitOfWork.CourierRepository.UpdateAsync(courierFromDB);
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
            #region General Exception
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("", ex.Message);
            //    editedModel.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
            //    editedModel.GovernorateList = await unitOfWork.GovernorateRepository.GetAllAsync();
            //    return View("Edit", editedModel);
            //}
            #endregion
            #region Custom Exception
            catch(DuplicateEntityException ex)
            {
                ModelState.AddModelError("CourierName", ex.Message);
                editedModel.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
                editedModel.GovernorateList = await unitOfWork.GovernorateRepository.GetAllAsync();
                return View("Edit", editedModel);
            }
            catch (EntityNotFoundException ex) {
                return NotFound(ex.Message);
            }
            #endregion


            editedModel.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
            editedModel.GovernorateList = await unitOfWork.GovernorateRepository.GetAllAsync();
            return View("Edit", editedModel);
            #region Manual Mapping
            //if (ModelState.IsValid)
            //{
            //    Couriers courierFromDB = await unitOfWork.SpecificCourierRepository.CourierWithDataById(editedModel.Id);
            //    courierFromDB.User.UserName = editedModel.CourierName;
            //    courierFromDB.User.Email = editedModel.CourierEmail;
            //    courierFromDB.User.PhoneNumber = editedModel.CourierPhone;
            //    courierFromDB.User.Address = editedModel.CourierAddress;
            //    courierFromDB.BranchId = editedModel.BranchId;
            //    courierFromDB.GovernorateId = editedModel.GovernorateId;
            //    courierFromDB.DiscountTypeOption = editedModel.DiscountTypeOptions.Value;
            //    courierFromDB.DiscountValue = editedModel.CompanyDiscountValue;
            //    await unitOfWork.CourierRepository.UpdateAsync(courierFromDB);
            //    await unitOfWork.SaveAsync();
            //    return RedirectToAction("Index");
            //}
            //editedModel.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
            //editedModel.GovernorateList = await unitOfWork.GovernorateRepository.GetAllAsync();
            //return View("Edit",editedModel);
            #endregion

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
