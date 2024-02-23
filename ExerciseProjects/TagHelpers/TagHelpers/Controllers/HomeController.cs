using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Diagnostics;

using TagHelpers.Models;

using static TagHelpers.Models.Repository;

namespace TagHelpers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository repository;

        public HomeController(ILogger<HomeController> logger, IRepository repo)
        {
            _logger = logger;
            repository = repo;
        }

        public IActionResult Index()
        {
            return View(repository.Products);
        }

        public IActionResult Index1()
        {
            return View(repository.Products);
        }

        #region Create
        [HttpGet] // Ä¬ÕJÐÐ ‘
        public ViewResult Create()
        {
            ViewBag.Quantity = new SelectList(repository.Products.Select(x => x.Quantity).Distinct());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            repository.AddProduct(product);
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        //public ViewResult Edit() => View("Create", repository.Products.Last());
        public ViewResult Edit()
        {
            ViewBag.Quantity = new SelectList(repository.Products.Select(x => x.Quantity).Distinct());
            return View("Create", repository.Products.Last());
        }
        #endregion

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
