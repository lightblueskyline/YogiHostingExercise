using DependencyInjection.Models;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;
using DependencyInjection.Models;

namespace DependencyInjection.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IRepository repository;
        //private ProductSum productSum;

        public HomeController(ILogger<HomeController> logger, IRepository repo)
        {
            // 構造函數 DI 昂貴，有些并非所有 Action 都使用
            _logger = logger;
            repository = repo;
            //this.productSum = productSum;
        }

        // Dependency Injection on Action Methods -> [FromServices] ProductSum productSum
        public IActionResult Index([FromServices] ProductSum productSum)
        {
            // 緊耦合 new Repository().Products
            //return View(new Repository().Products);

            ViewBag.Total = productSum.Total;
            ViewBag.HomeControllerGuid = repository.ToString();
            ViewBag.TotalGuid = productSum.Repository.ToString();
            // 通過 DI 去耦
            return View(repository.Products);
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
    }
}
