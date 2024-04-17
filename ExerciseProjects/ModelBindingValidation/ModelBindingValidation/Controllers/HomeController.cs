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
            /**
             * 依據畫面
             * @model ModelBindingValidation.Models.Employee
             * HomeAddress.City
             * HomeAddress.Country
             * 判斷需要添加前綴 [Bind(Prefix = nameof(Employee.HomeAddress))]
             */
            return View(personAddress);
        }

        [HttpPost]
        public IActionResult DisplayPerson1([Bind(nameof(PersonAddress.City), Prefix = nameof(Employee.HomeAddress))] PersonAddress personAddress)
        {
            /**
             * 模型綁定時，只綁定至屬性 nameof(PersonAddress.City)
             */
            return View("DisplayPerson", personAddress);
        }
        #endregion

        #region Advanced Model Binding

        #region Model Binding in Arrays
        public IActionResult Places(string[] places) => View(places);
        #endregion

        #region Model Binding in Collections
        public IActionResult Places1(List<string> places) => View(places);
        #endregion

        #region Model Binding in Collections of Complex Types
        public IActionResult Address() => View();

        [HttpPost]
        public IActionResult Address(List<PersonAddress> address) => View(address);
        #endregion

        #region FromForm
        public IActionResult FromFormExample() => View();

        [HttpPost]
        public IActionResult FromFormExample([FromForm] Employee model)
        {
            ViewBag.Message = "Employee data received";
            return View();
        }

        public IActionResult FromFormExample1() => View();

        [HttpPost]
        public Employee FromFormExample1([FromForm] Employee model) => model;
        #endregion

        #region FromBody
        public IActionResult Body() => View();

        [HttpPost]
        public Employee Body([FromBody] Employee model) => model;
        #endregion

        #region FromQuery

        #endregion

        #region FromHeader
        public string Header([FromHeader(Name = "User-Agent")] string accept) => $"Header: {accept}";

        public IActionResult FullHeader(FullHeader model) => View(model);
        #endregion

        #region FromRoute
        public IActionResult FromRouteExample() => View();

        [HttpPost]
        public string FromRouteExample([FromRoute] string id) => id;
        #endregion

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
