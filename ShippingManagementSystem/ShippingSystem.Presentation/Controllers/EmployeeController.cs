using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Presentation.ViewModels.EmployeeVM;
using ShippingSystem.Presentation.ViewModels.OrderVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public EmployeeController(IUnitOfWork unitOfWork,
                                  UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager
            )
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        [Authorize(Policy = "ViewEmployeeHome")]
        public async Task<IActionResult> EmployeeHome()
        {
            List<Orders> orderList = await unitOfWork.OrderRepository.GetAllAsync();
            OrdersHomeVM mappedEmployeeeHome = new OrdersHomeVM()
            {
                OrderCountByStatus = orderList.GroupBy(o => o.OrderStatusId).ToDictionary(order => order.Key, order => order.Count()),
                OrderStatusList = await unitOfWork.OrderStatusRepository.GetAllAsync()
            };
            return View("OrdersHome",mappedEmployeeeHome);
        }
        [HttpGet]
        [Authorize(Policy = "ViewEmployees")]
        public async Task<IActionResult> Index()
        {
            List<Employees> empList = await unitOfWork.SpecificEmployeeRepository.EmpListWithBranch();
            List<GetEmployeeListViewModel> empListMapped = empList.Select(e => new GetEmployeeListViewModel
            {
                EmployeeId=e.Id,
                EmployeeName=e.User.UserName,
                EmployeeEmail=e.User.Email,
                EmployeePhone=e.User.PhoneNumber,
                BranchName=e.Branch.Name,
                IsActive=e.IsActive,

            }).ToList();
            
            return View("Index",empListMapped);
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
            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new ApplicationUser()
                {
                    UserName=newEmpFromUser.EmployeeName,
                    Email=newEmpFromUser.EmployeeEmail,
                    PhoneNumber=newEmpFromUser.EmployeePhone,
                    Address=newEmpFromUser.EmployeeAddress,
                };
                IdentityResult result =await userManager.CreateAsync(newUser, newEmpFromUser.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "Employee");
                    //await signInManager.SignInAsync(newUser, false);
                    Employees newEmp = new Employees()
                    {
                        BranchId = newEmpFromUser.BranchId,
                        User = newUser
                    };
                    await unitOfWork.EmployeeRepository.AddAsync(newEmp);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");

                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
              
            }
            newEmpFromUser.BranchList = await unitOfWork.BranchRepository.GetAllAsync();
            return View("Add",newEmpFromUser);
        }
        [HttpGet]
        [Authorize(Policy = "EditEmployee")]
        public async Task<IActionResult> Edit(int Id)
        {
            Employees empById = await unitOfWork.SpecificEmployeeRepository.EmpWithUserById(Id);
            EmployeeEditViewModel mappedEmployee = new EmployeeEditViewModel()
            {
                EmployeeName=empById.User.UserName,
                EmployeeEmail=empById.User.Email,
                EmployeePhone=empById.User.PhoneNumber,
                EmployeeAddress=empById.User.Address,
                BranchId=empById.BranchId,
            };
            mappedEmployee.BranchList =await unitOfWork.BranchRepository.GetAllAsync();
            return View("Edit",mappedEmployee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EmployeeEditViewModel editModelFromUser)
        {
            if (ModelState.IsValid)
            {
                Employees empFromDB = await unitOfWork.SpecificEmployeeRepository.EmpWithUserById(editModelFromUser.Id);

                empFromDB.User.UserName = editModelFromUser.EmployeeName;
                empFromDB.User.PhoneNumber = editModelFromUser.EmployeePhone;
                empFromDB.User.Address = editModelFromUser.EmployeeAddress;
                empFromDB.User.Email = editModelFromUser.EmployeeEmail;
                empFromDB.BranchId = editModelFromUser.BranchId;
                empFromDB.Id= editModelFromUser.Id;
                await unitOfWork.EmployeeRepository.UpdateAsync(empFromDB);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Edit",editModelFromUser);
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
