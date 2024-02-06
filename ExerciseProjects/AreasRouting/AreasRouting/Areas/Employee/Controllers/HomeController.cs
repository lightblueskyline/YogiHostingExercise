using Microsoft.AspNetCore.Mvc;

namespace AreasRouting.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
