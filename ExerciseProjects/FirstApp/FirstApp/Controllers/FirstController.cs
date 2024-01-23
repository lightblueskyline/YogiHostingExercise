using Microsoft.AspNetCore.Mvc;
using FirstApp.Models;

namespace FirstApp.Controllers
{
    public class FirstController : Controller
    {
        public string Index()
        {
            return "Hello World";
        }

        public IActionResult Hello()
        {
            // ViewBag -> 動態對象
            // ViewBag -> 可以被分配任何類型的變量
            ViewBag.Message = "Hello World";
            return View();
        }

        public IActionResult Info()
        {
            Person person = new Person();
            person.Name = "John";
            person.Age = 18;
            person.Location = "United States";
            
            return View(person);
        }
    }
}
