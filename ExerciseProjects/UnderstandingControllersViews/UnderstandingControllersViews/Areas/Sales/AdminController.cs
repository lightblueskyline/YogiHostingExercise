using Microsoft.AspNetCore.Mvc;

namespace UnderstandingControllersViews.Areas.Sales
{
    [Area("Sales")]
    public class AdminController : Controller
    {
        public IActionResult List()
        {
            return View("Show");
        }
    }
}
