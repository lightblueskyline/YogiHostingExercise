using Microsoft.AspNetCore.Mvc;

namespace UnderstandingControllersViews.Controllers
{
    public class CityController : Controller
    {
        public IActionResult NewYork()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewYork(string name, string sex)
        {
            return View();
        }

        public IActionResult Boston()
        {
            return View();
        }
    }
}
