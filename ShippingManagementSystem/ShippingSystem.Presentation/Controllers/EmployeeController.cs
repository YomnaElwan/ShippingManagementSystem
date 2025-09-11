using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels.EmployeeVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IGenericService<Branches> branchService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IGenericService<Employees> empService;
        private readonly IEmployeeService employeeService;

        public EmployeeController(IGenericService<Branches> branchService,
                                  UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager,
                                  IGenericService<Employees> empService,
                                  IEmployeeService employeeService)
        {
            this.branchService = branchService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.empService = empService;
            this.employeeService = employeeService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Employees> empList = await employeeService.EmpListWithBranch();
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
        public async Task<IActionResult> Add()
        {
            List<Branches> branches = await branchService.GetAllAsync();
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
                    await signInManager.SignInAsync(newUser, false);
                    Employees newEmp = new Employees()
                    {
                        BranchId = newEmpFromUser.BranchId,
                        User = newUser
                    };
                    await empService.AddAsync(newEmp);
                    await empService.SaveAsync();
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
            newEmpFromUser.BranchList = await branchService.GetAllAsync();
            return View("Add",newEmpFromUser);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            Employees empById = await employeeService.EmpWithUserById(Id);
            EmployeeEditViewModel mappedEmployee = new EmployeeEditViewModel()
            {
                EmployeeName=empById.User.UserName,
                EmployeeEmail=empById.User.Email,
                EmployeePhone=empById.User.PhoneNumber,
                EmployeeAddress=empById.User.Address,
                BranchId=empById.BranchId,
            };
            mappedEmployee.BranchList =await branchService.GetAllAsync();
            return View("Edit",mappedEmployee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EmployeeEditViewModel editModelFromUser)
        {
            if (ModelState.IsValid)
            {
                Employees empFromDB = await employeeService.EmpWithUserById(editModelFromUser.Id);

                empFromDB.User.UserName = editModelFromUser.EmployeeName;
                empFromDB.User.PhoneNumber = editModelFromUser.EmployeePhone;
                empFromDB.User.Address = editModelFromUser.EmployeeAddress;
                empFromDB.User.Email = editModelFromUser.EmployeeEmail;
                empFromDB.BranchId = editModelFromUser.BranchId;
                empFromDB.Id= editModelFromUser.Id;
                await empService.UpdateAsync(empFromDB);
                await empService.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Edit",editModelFromUser);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            await empService.DeleteAsync(Id);
            await empService.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
