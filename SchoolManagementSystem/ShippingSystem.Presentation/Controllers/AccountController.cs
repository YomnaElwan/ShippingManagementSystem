using Microsoft.AspNetCore.Mvc;

namespace ShippingSystem.Presentation.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
