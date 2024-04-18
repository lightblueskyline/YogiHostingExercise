using Filters.CustomFilters;
using Filters.Models;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace Filters.Controllers
{
    //[RequireHttps] // 會阻止非 HTTPS 鏈接的訪問
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //[RequireHttps] // 會阻止非 HTTPS 鏈接的訪問
        [HttpsOnly] // ASP.NET Core Authorization Filters
        [TimeElapsed] // ASP.NET Core Action Filters
        public string Index()
        {
            //return View();
            return "This is the Index action on the Home controller";
        }

        [TimeElapsedAsync] // ASP.NET Core Action Filters
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[ChangeView] // ASP.NET Core Result Filters
        //[ChangeViewAsync] // ASP.NET Core Result Filters
        [HybridActRes] // Hybrid Action/Result Filter
        public IActionResult Message() => View();

        [CatchError] // ASP.NET Core Exception Filters
        public IActionResult Exception(int? id)
        {
            if (id == null)
            {
                throw new Exception("Error, ID can not be null");
            }
            else
            {
                return View((object)$"The value is {id}");
            }
        }
    }
}
