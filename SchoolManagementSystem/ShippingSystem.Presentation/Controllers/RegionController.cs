using Microsoft.AspNetCore.Mvc;

namespace ShippingSystem.Presentation.Controllers
{
    public class RegionController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
