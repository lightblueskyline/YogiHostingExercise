using Identity.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager)
        {
            _logger = logger;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            AppUser? user = await this.userManager.GetUserAsync(HttpContext.User);
            string message = "Hello";
            if (user != null)
            {
                message = $"Hello {user.UserName}";
            }
            return View((object)message);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[Authorize]
        [Authorize(Roles = "Manager")] // ASP.NET Core Identity Role based Authentication
        public async Task<IActionResult> Secured()
        {
            //return View((object)"Hello");

            #region ASP.NET Core Identity Role based Authentication
            AppUser? user = await this.userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                string message = $"Hello {user.UserName}";
                return View((object)message);
            }
            else
            {
                return View((object)"No User");
            }
            #endregion
        }
    }
}
