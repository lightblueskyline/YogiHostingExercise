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
            // ���캯�� DI ���F����Щ�������� Action ��ʹ��
            _logger = logger;
            repository = repo;
            //this.productSum = productSum;
        }

        // Dependency Injection on Action Methods -> [FromServices] ProductSum productSum
        public IActionResult Index([FromServices] ProductSum productSum)
        {
            // �o��� new Repository().Products
            //return View(new Repository().Products);

            ViewBag.Total = productSum.Total;
            ViewBag.HomeControllerGuid = repository.ToString();
            ViewBag.TotalGuid = productSum.Repository.ToString();
            // ͨ�^ DI ȥ��
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
