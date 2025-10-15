using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels.AccountVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
       
        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager
                                 )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel userFromRequest)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new ApplicationUser()
                {
                    UserName = userFromRequest.Name,
                    PasswordHash = userFromRequest.Password,
                    Address = userFromRequest.Address
                };
                IdentityResult result = await userManager.CreateAsync(newUser, userFromRequest.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser,"Admin");
                    await signInManager.SignInAsync(newUser, false);
                    return RedirectToAction("Index", "Governorate");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View("Register", userFromRequest);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userFromDB = await userManager.FindByNameAsync(loginUser.Name);
                if (userFromDB != null)
                {
                    bool checkPassword = await userManager.CheckPasswordAsync(userFromDB, loginUser.Password);
                    if (checkPassword == true)
                    {

                        await signInManager.SignInAsync(userFromDB, loginUser.RememberMe);
                        var claims = await userManager.GetClaimsAsync(userFromDB);
                        foreach (var claim in claims)
                        {
                            Console.WriteLine($"{claim.Type} - {claim.Value}");
                        }
                        return RedirectToAction("Index", "City");
                    }
                }
                ModelState.AddModelError("", "Invalid Name OR Password");
            }
            return View("Login",loginUser);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
