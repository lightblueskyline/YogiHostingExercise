using GloLoc.Models;

using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace GloLoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index1() => View();

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region CookieRequestCultureProvider
        public IActionResult Cookie() => View();

        [HttpPost]
        public IActionResult Cookie(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) });
            return RedirectToAction("Cookie");
        }
        #endregion

        #region AcceptLanguageHeaderRequestCultureProvider
        public IActionResult Browser() => View();
        #endregion
    }
}
