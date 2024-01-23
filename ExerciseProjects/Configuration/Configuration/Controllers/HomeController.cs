using Configuration.Services;
using Configuration.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Configuration.Controllers
{
    public class HomeController : Controller
    {
        private TotalUsers totalUsers;
        private IWebHostEnvironment hostingEnvironment;
        private IConfiguration settings;
        private IOptions<OpenWeatherMapApi> options;

        public HomeController(TotalUsers tu, IWebHostEnvironment environment, IConfiguration configuration, IOptions<OpenWeatherMapApi> options)
        {
            // 通過依賴注入，注入服務 TotalUsers
            totalUsers = tu;
            // 注入 IWebHostEnvironment
            hostingEnvironment = environment;
            // 注入 IConfiguration
            settings = configuration;
            // 注入 IOptions<OpenWeatherMapApi>
            this.options = options;
        }

        public string Index()
        {
            if (hostingEnvironment.IsProduction())
            {
                // ...
            }
            bool responseEditingMiddleware = Convert.ToBoolean(settings["Middleware:EnableResponseEditingMiddleware"]);
            return $"Total Users are: {totalUsers.TUsers()} {System.Environment.NewLine}WebRootPath: {hostingEnvironment.WebRootPath} {System.Environment.NewLine}Middleware:EnableResponseEditingMiddleware: {responseEditingMiddleware}";
        }

        public IActionResult Exception()
        {
            throw new System.NullReferenceException();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
