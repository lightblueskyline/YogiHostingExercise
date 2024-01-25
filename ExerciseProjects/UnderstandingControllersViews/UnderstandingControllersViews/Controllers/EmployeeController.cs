using Microsoft.AspNetCore.Mvc;

using UnderstandingControllersViews.Models;

namespace UnderstandingControllersViews.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action Methods returning String type Model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(int id, string name)
        {
            string welcomeMessage = $"Welcome Employee: {name} with id: {id}";
            return View((object)welcomeMessage);
        }

        public IActionResult Detail()
        {
            return View();
        }

        /// <summary>
        /// Action Methods returning Class type Model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Detail(int id, string name)
        {
            Employee emp = new Employee();
            emp.Id = id;
            emp.Name = name;
            emp.Salary = 1000;
            emp.Designation = "Manager";
            emp.Address = "New York";
            return View(emp);
        }
    }
}
