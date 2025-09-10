//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using ShippingSystem.Application.Interfaces;
//using ShippingSystem.Domain.Entities;
//using ShippingSystem.Presentation.ViewModels;

//namespace ShippingSystem.Presentation.Controllers
//{
//    public class UserController : Controller
//    {
//        private readonly RoleManager<IdentityRole> roles;
//        private readonly UserManager<ApplicationUser> userManager;
//        private readonly SignInManager<ApplicationUser> signInManager;
//        private readonly IGenericService<Branches> branchService;
//        public UserController(RoleManager<IdentityRole> roles,  
//                              UserManager<ApplicationUser> userManager,
//                              SignInManager<ApplicationUser> signInManager,
//                              IGenericService<Branches> branchService)
//        {
//            this.roles = roles;
//            this.userManager = userManager;
//            this.signInManager = signInManager;
//            this.branchService = branchService;
//        }
//        public async Task<IActionResult> Add()
//        {
//            List<IdentityRole> rolesList = roles.Roles.ToList();
//            List<Branches> branchList =await branchService.GetAllAsync();

//            var vm = new AddUserViewModel()
//            {
//                Roles = rolesList,
//                Branches=branchList,
//            };
//            return View("Add",vm);
//        }
//        public async Task <IActionResult> SaveAdd(AddUserViewModel newUserFromRequest)
//        { 
//            if (string.IsNullOrEmpty(newUserFromRequest.RoleId))
//            {
//                ModelState.AddModelError("RoleId", "You Must Choose a Role!");
//            }
//            if (newUserFromRequest.BranchId == 0)
//            {
//                ModelState.AddModelError("BranchId", "You Must Choose a Branch");
//            }

//            if (ModelState.IsValid)
//            {
//                ApplicationUser newUser = new ApplicationUser() {
//                UserName=newUserFromRequest.UserName,
//                RoleId =newUserFromRequest.RoleId,
//                Email=newUserFromRequest.Email,
//                PhoneNumber=newUserFromRequest.PhoneNumber,
//                BranchId=newUserFromRequest.BranchId,
//                Address=newUserFromRequest.UserAddress,
                
//                };
//                IdentityResult result = await userManager.CreateAsync(newUser, newUserFromRequest.Password);
//                if (result.Succeeded)
//                {
//                    var role = await roles.FindByIdAsync(newUserFromRequest.RoleId);
//                    if (role != null)
//                    {
//                       await userManager.AddToRoleAsync(newUser, role.Name);
//                    }
//                    await signInManager.SignInAsync(newUser, false);
//                    return RedirectToAction("Index", "Home");
//                }
//                else
//                {
//                    foreach(var err in result.Errors)
//                    {
//                        ModelState.AddModelError("", err.Description);
//                    }
//                }
//            }
//            newUserFromRequest.Branches = await branchService.GetAllAsync();
//            newUserFromRequest.Roles = roles.Roles.ToList();
//            ViewBag.SelectedRole = newUserFromRequest.RoleId;
//            return View("Add",newUserFromRequest);
//        }
//        public async Task<IActionResult> LoadRoleView(string RoleId)
//        {
//            AddUserViewModel lists = new AddUserViewModel()
//            {
//                Branches = await branchService.GetAllAsync(),
//                Roles = roles.Roles.ToList()

//            };
//            var role = roles.FindByIdAsync(RoleId).Result;
//            if (role == null)
//            {
//                return Content("There is no role has this role id");
//            }
//            switch (role.Name)
//            {
//                case "Admin":
//                    return PartialView("_AdminPartialView");
//                case "Employee":
//                    return PartialView("_EmployeePartialView",lists);
//                case "Merchant":
//                    return PartialView("_MerchantPartialView");
//                case "Courier":
//                    return PartialView("_CourierPartialView");
//                default:
//                    return Content("Please Select a Valid Role");
                 

//            }
           
//        }
       
//    }
//}
