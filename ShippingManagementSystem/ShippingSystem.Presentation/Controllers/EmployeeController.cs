using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Infrastructure.Repositories;
using ShippingSystem.Presentation.ViewModels.EmployeeVM;
using ShippingSystem.Presentation.ViewModels.OrderVM;
using System.Linq.Expressions;

namespace ShippingSystem.Presentation.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;
        public EmployeeController(IUnitOfWork unitOfWork,
                                  UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager,
                                  IMapper mapper
            )
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Policy = "ViewEmployeeHome")]
        public async Task<IActionResult> EmployeeHome()
        {
            List<Orders> orderList = await unitOfWork.OrderRepository.GetAllAsync();
            OrdersHomeVM mappedEmployeeeHome = new OrdersHomeVM()
            {
                OrderCountByStatus = orderList.GroupBy(o => o.OrderStatusId).Select(o => new { statusId = o.Key, Count = o.Count() }).ToDictionary(order => order.statusId, order => order.Count),// Best -- Count in SQL 
                //OrderCountByStatus = orderList.GroupBy(o => o.OrderStatusId).ToDictionary(order => order.Key, order => order.Count()), //Count in C#
                OrderStatusList = await unitOfWork.OrderStatusRepository.GetAllAsync()
            };
            return View("OrdersHome",mappedEmployeeeHome);
        }
        [HttpGet]
        [Authorize(Policy = "ViewEmployees")]
        public async Task<IActionResult> Index()
        {
            List<Employees> empList = await unitOfWork.SpecificEmployeeRepository.EmpListWithBranch();
            #region Auto Mapper
            List<GetEmployeeListViewModel> mappedReadEmployees = mapper.Map<List<GetEmployeeListViewModel>>(empList);
            return View("Index", mappedReadEmployees);
            #endregion
            #region manual mapping
            //List<GetEmployeeListViewModel> empListMapped = empList.Select(e => new GetEmployeeListViewModel
            //{
            //    EmployeeId=e.Id,
            //    EmployeeName=e.User.UserName,
            //    EmployeeEmail=e.User.Email,
            //    EmployeePhone=e.User.PhoneNumber,
            //    BranchName=e.Branch.Name,
            //    IsActive=e.IsActive,

            //}).ToList();
            //return View("Index",empListMapped);
            #endregion
        }
        [HttpGet]
        [Authorize(Policy = "AddNewEmployee")]
        public async Task<IActionResult> Add()
        {
            List<Branches> branches = await unitOfWork.BranchRepository.GetAllAsync();
            AddEmployeeViewModel modelIncludeBranchList = new AddEmployeeViewModel() {
               BranchList=branches
            };

            return View("Add",modelIncludeBranchList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNew(AddEmployeeViewModel newEmpFromUser)
        {
            if (newEmpFromUser.BranchId == 0)
            {
                ModelState.AddModelError("BranchId", "You Must Select a Branch Name");
            }
            if (!ModelState.IsValid)
            {
                newEmpFromUser.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
                return View("Add", newEmpFromUser);
            }
            try
            {
                ApplicationUser newUser = mapper.Map<ApplicationUser>(newEmpFromUser);
                IdentityResult result = await userManager.CreateAsync(newUser, newEmpFromUser.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "Employee");
                    //await signInManager.SignInAsync(newUser, false);
                    Employees newEmp = mapper.Map<Employees>(newEmpFromUser);
                    newEmp.User = newUser;
                    await unitOfWork.EmployeeRepository.AddAsync(newEmp);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                newEmpFromUser.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
                return View("Add", newEmpFromUser);
            }
            newEmpFromUser.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
            return View("Add", newEmpFromUser);
        }
        [HttpGet]
        [Authorize(Policy = "EditEmployee")]
        public async Task<IActionResult> Edit(int Id)
        {
            Employees empById = await unitOfWork.SpecificEmployeeRepository.EmpWithUserById(Id);
            #region Manual Mapping
            //EmployeeEditViewModel mappedEmployee = new EmployeeEditViewModel()
            //{
            //    EmployeeName = empById.User.UserName,
            //    EmployeeEmail = empById.User.Email,
            //    EmployeePhone = empById.User.PhoneNumber,
            //    EmployeeAddress = empById.User.Address,
            //    BranchId = empById.Branch.Id,
            //};
            //mappedEmployee.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
            //return View("Edit", mappedEmployee);
            #endregion
            #region Auto Mapper
            EmployeeEditViewModel mappedEmployee = mapper.Map<EmployeeEditViewModel>(empById);
            mappedEmployee.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
            return View("Edit", mappedEmployee);
            #endregion
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EmployeeEditViewModel editModelFromUser)
        {
            if (!ModelState.IsValid)
            {
                editModelFromUser.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
                return View("Edit", editModelFromUser);
            }
            try
            {
                var getEmpFromDB = await unitOfWork.SpecificEmployeeRepository.EmpWithUserById(editModelFromUser.Id);
                var getUserByName = await userManager.FindByNameAsync(editModelFromUser.EmployeeName);
                var getUserByEmail = await userManager.FindByEmailAsync(editModelFromUser.EmployeeEmail);
                if(getUserByName!=null && getEmpFromDB.UserId!=getUserByName.Id)
                {
                    throw new Exception($"Name '{editModelFromUser.EmployeeName}' is already existing!");
                }
                if(getUserByEmail!=null && getEmpFromDB.UserId != getUserByEmail.Id)
                {
                    throw new Exception($"Email '{editModelFromUser.EmployeeEmail}' is already existing!");
                }
                mapper.Map(editModelFromUser, getEmpFromDB.User);
                IdentityResult identity = await userManager.UpdateAsync(getEmpFromDB.User);
                if (identity.Succeeded)
                {
                    mapper.Map(editModelFromUser, getEmpFromDB);
                    await unitOfWork.EmployeeRepository.UpdateAsync(getEmpFromDB);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var err in identity.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                editModelFromUser.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
                return View("Edit", editModelFromUser);
            }
            editModelFromUser.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
            return View("Edit", editModelFromUser);
            #region Manual Mapping
            //if (ModelState.IsValid)
            //{
            //    Employees empFromDB = await unitOfWork.SpecificEmployeeRepository.EmpWithUserById(editModelFromUser.Id);

            //    empFromDB.User.UserName = editModelFromUser.EmployeeName;
            //    empFromDB.User.PhoneNumber = editModelFromUser.EmployeePhone;
            //    empFromDB.User.Address = editModelFromUser.EmployeeAddress;
            //    empFromDB.User.Email = editModelFromUser.EmployeeEmail;
            //    empFromDB.BranchId = editModelFromUser.BranchId;
            //    empFromDB.Id = editModelFromUser.Id;
            //    await unitOfWork.EmployeeRepository.UpdateAsync(empFromDB);
            //    await unitOfWork.SaveAsync();
            //    return RedirectToAction("Index");
            //}
            //return View("Edit", editModelFromUser);
            #endregion
        }
        [Authorize(Policy = "DeleteEmployee")]

        public async Task<IActionResult> Delete(int Id)
        {
            await unitOfWork.EmployeeRepository.DeleteAsync(Id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
