using Filters.CustomFilters;

using Microsoft.AspNetCore.Mvc;

namespace Filters.Controllers
{
    [ShowMessage("Controller", Order = 2)]
    public class OrderController : Controller
    {
        [ShowMessage("Action", Order = 1)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
