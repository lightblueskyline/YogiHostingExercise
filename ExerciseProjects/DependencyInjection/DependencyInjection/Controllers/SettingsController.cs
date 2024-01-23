using Microsoft.AspNetCore.Mvc;
using DependencyInjection.Models;
using Microsoft.Extensions.Options;

namespace DependencyInjection.Controllers
{
    /// <summary>
    /// Dependency Injection of JSON files
    /// </summary>
    public class SettingsController : Controller
    {
        private readonly MyJson _settings;

        public SettingsController(IOptions<MyJson> settingsOptions)
        {
            _settings = settingsOptions.Value;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = _settings.Title;
            ViewData["Version"] = _settings.Version;
            return View();
        }

        public IActionResult Show()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}
