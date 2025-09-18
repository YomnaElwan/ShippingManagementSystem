using Microsoft.AspNetCore.Mvc;

namespace ShippingSystem.Presentation.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
