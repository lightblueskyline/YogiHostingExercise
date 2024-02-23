using Microsoft.AspNetCore.Mvc;

using ModelBindingValidation.Models;

using System.Diagnostics;

namespace ModelBindingValidation.Controllers
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

        public IActionResult Index(int? id)
        {
            if (id == null)
            {
                id = 1;
            }

            return View(repository[Convert.ToInt32(id)]);
        }

        #region Create
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Employee model)
        {
            string? hostNo = Request.Form["HomeAddress.HouseNumber"];
            return View("Index", model);
        }
        #endregion

        #region DisplayPerson
        [HttpPost]
        public IActionResult DisplayPerson([Bind(Prefix = nameof(Employee.HomeAddress))] PersonAddress personAddress)
        {
            return View(personAddress);
        }

        [HttpPost]
        public IActionResult DisplayPerson1([Bind(nameof(PersonAddress.City), Prefix = nameof(Employee.HomeAddress))] PersonAddress personAddress)
        {
            return View("DisplayPerson", personAddress);
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
