using Microsoft.AspNetCore.Mvc;

namespace RouteLinks.Controllers
{
    [Route("News/[controller]/USA/[action]/{id?}")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
