using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using UnderstandingControllersViews.Models;

namespace UnderstandingControllersViews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            hostingEnvironment = environment;
        }

        public IActionResult Index()
        {
            ViewData["Name"] = "Yogi";
            ViewData["Address"] = new Address()
            {
                HouseNo = "Steve",
                City = "Hudson",
            };

            return View();
        }

        /// <summary>
        /// ASP.NET Core Controller for File Uploading
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile photo)
        {
            // 入參名稱必須同 <input type="file" class="form-control" name="photo" /> (name 值相同)
            // code to save the uploaded file in wwwroot folder
            using (var stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, photo.FileName), FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            ViewData["Name"] = "Yogi";
            ViewData["Address"] = new Address()
            {
                HouseNo = "Steve",
                City = "Hudson",
            };

            return View();
        }

        /// <summary>
        /// FormData objects
        /// 1. Controllers Request Property – Request.Form
        /// </summary>
        /// <returns></returns>
        public IActionResult ReceivedDataByRequest()
        {
            string name = Request.Form["name"];
            string sex = Request.Form["sex"];

            return View("ReceivedDataByRequest", $"{name} sex is {sex}");
        }

        /// <summary>
        /// 2. Receive Data in the “Parameters” of Action Methods
        /// 3. Passing data from Query String
        /// </summary>
        /// <returns></returns>
        public IActionResult ReceivedDataByParameter(string name, string sex)
        {
            return View("ReceivedDataByParameter", $"{name} sex is {sex}");
        }

        /// <summary>
        /// 4. Transfer View’s data to Controller with Model Binding
        /// </summary>
        /// <returns></returns>
        public IActionResult ReceivedDataByModelBinding(Person person)
        {
            return View("ReceivedDataByModelBinding", person);
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

        public IActionResult CallSharedView()
        {
            return View();
        }

        public IActionResult TestLayout()
        {
            ViewBag.Title = "Welcome to TestLayout";
            return View();
        }

        public IActionResult Joke()
        {
            return View();
        }
    }
}
