using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Presentation.ViewModels.PermissionsVM;
using ShippingSystem.Presentation.ViewModels.RoleVM;
using System.Security.Claims;

namespace ShippingSystem.Presentation.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> _roleManager)
        {
            this._roleManager = _roleManager;
        }
        [HttpGet]
        [Authorize(Policy = "ViewRoles")]
        public async Task<IActionResult> Index()
        {
            List<IdentityRole> roleList = await _roleManager.Roles.ToListAsync();
            return View("Index",roleList);
        }
        [HttpGet]
        [Authorize(Policy = "AddNewRole")]
        public IActionResult Add()
        {
            return View("Add");
        }
        //Add Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoleViewModel roleView)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole() { 
                       Name=roleView.RoleName,
                };
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index","Role");
                     
                }
                else
                {
                    foreach(var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View("Add", roleView);
           
        }
        //Manage Claims
        [Authorize(Policy = "ManageRole")]
        public async Task<IActionResult> Manage(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();
            var claims = await _roleManager.GetClaimsAsync(role);
            var model = new RoleClaimsVM
            {
                RoleId = roleId,
                RoleName = role.Name,
                RoleClaims = claims.Select(c => new RoleClaimItemVM
                {
                    ClaimValue = c.Value,
                    ClaimType = c.Type,

                }).ToList(),
                AllPermissions = new List<string>
                {
                    //Account
                    "Register",
                    //Branches
                    "ViewBranches","ViewBranchDetails","AddNewBranch","EditBranch","DeleteBranch",
                    //City
                    "ViewCities","ViewCityDetails","AddNewCity","EditCity","DeleteCity",
                    //Governorate
                    "ViewGovernorates","ViewGovernorateDetails","AddNewGovernorate","EditGovernorate","DeleteGovernorate",
                    //Region
                    "ViewRegions","ViewRegionDetails","AddNewRegion","EditRegion","DeleteRegion",
                    //Role
                    "ViewRoles","AddNewRole","ManageRole","AddClaim","DeleteClaim","DeleteRole",
                    //ShippingType
                    "ViewShippingTypes","AddNewShippingType","EditShippingType","DeleteShippingType",
                    //PaymentMethod
                    "ViewPaymentMethods","AddNewPaymentMethod","EditPaymentMethod","DeletePaymentMethod",
                    //WeightSettings
                    "ViewWeightSettings","ViewWeightSettingsDetails","AddNewWeightSetting","EditWeightSetting","DeleteWeightSetting",                    
                    //OrderStatus
                    "ViewAllOrderStatus","AddOrderStatus","EditOrderStatus","DeleteOrderStatus",
                    //Courier
                    "ViewCouriers","ViewCourierDetails","AddNewCourier","EditCourier","DeleteCourier",
                    //Employee
                    "ViewEmployeeHome","ViewEmployees","AddNewEmployee","EditEmployee","DeleteEmployee",
                    //Merchant
                    "ViewMerchantHome","ViewMerchants","ViewMerchantDetails","AddNewMerchant","EditMerchant","DeleteMerchant",
                    //Order
                    "OrdersHome","AllOrders","AddNewOrder","EditOrderStatus","EditOrder","DeleteOrder","OrderReport",    
                }
            };
            return View(model);
        }
        //Add Claims
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AddClaim")]
        public async Task<IActionResult> AddClaim(string roleId, string permission)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();
            await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            return RedirectToAction("Manage", new { roleId });
        }
        //Delete Claims
        [HttpPost]
        [Authorize(Policy = "DeleteClaim")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClaim(string roleId, string permission)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();
            var claims = await _roleManager.GetClaimsAsync(role);
            var claim = claims.FirstOrDefault(c => c.Type == "Permission" && c.Value == permission);
            if (claim != null)
                await _roleManager.RemoveClaimAsync(role, claim);
            return RedirectToAction("Manage", new { roleId });
        }
        //Delete Role
        [Authorize(Policy= "DeleteRole")]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }
    }
}
