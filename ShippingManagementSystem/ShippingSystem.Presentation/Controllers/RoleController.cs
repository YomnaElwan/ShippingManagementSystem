using Microsoft.AspNetCore.Mvc;

namespace ShippingSystem.Presentation.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
