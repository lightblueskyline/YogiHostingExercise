using Microsoft.AspNetCore.Mvc;
using FirstApp.Models;

namespace FirstApp.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View(Repository.AllEmployees);
        }

        #region CREATE
        // HTTP GET VERSION
        public IActionResult Create()
        {
            return View();
        }

        // HTTP POST VERSION
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                // 模型驗證
                Repository.Create(employee);
                return View("Thanks", employee);
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region UPDATE
        public IActionResult Update(string empname)
        {
            Employee employee = Repository.AllEmployees.Where(x => x.Name == empname).FirstOrDefault();
            return View(employee);
        }

        [HttpPost]
        public IActionResult Update(Employee employee, string empname)
        {
            if (ModelState.IsValid)
            {
                // 模型驗證
                Repository.AllEmployees.Where(x => x.Name == empname).FirstOrDefault().Age = employee.Age;
                Repository.AllEmployees.Where(x => x.Name == empname).FirstOrDefault().Salary = employee.Salary;
                Repository.AllEmployees.Where(x => x.Name == empname).FirstOrDefault().Department = employee.Department;
                Repository.AllEmployees.Where(x => x.Name == empname).FirstOrDefault().Sex = employee.Sex;
                Repository.AllEmployees.Where(x => x.Name == empname).FirstOrDefault().Name = employee.Name;

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region DELETE
        [HttpPost]
        public IActionResult Delete(string empname)
        {
            Employee employee = Repository.AllEmployees.Where(x => x.Name == empname).FirstOrDefault();
            Repository.Delete(employee);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
