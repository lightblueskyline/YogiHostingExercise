using GloLoc.Models;

using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using System.Diagnostics;

namespace GloLoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
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

        #region Controllers localization using Resource files
        public IActionResult JobApplicationIndex() => View();

        [HttpPost]
        public IActionResult JobApplicationIndex(JobApplication jobApplication)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = _localizer["Your application is accepted"];
            }

            return View();
        }
        #endregion

        #region Portable Object(PO) Files
        public IActionResult POFilesIndex()
        {
            // Reading translated values from PO files in Controllers
            string translatedString = _localizer["Hello world!"];

            return View();
        }
        #endregion
    }
}
