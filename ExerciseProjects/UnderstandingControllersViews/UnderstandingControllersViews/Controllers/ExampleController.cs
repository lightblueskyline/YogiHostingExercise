using Microsoft.AspNetCore.Mvc;

using UnderstandingControllersViews.Models;

namespace UnderstandingControllersViews.Controllers
{
    /// <summary>
    /// Passing Data from Action Method to View
    /// </summary>
    public class ExampleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// ViewBag
        /// </summary>
        /// <returns></returns>
        public IActionResult ViewBagExample()
        {
            // 注意：重定向之後 ViewBag 值會丟失
            ViewBag.CurrentDateTime = DateTime.Now;
            ViewBag.CurrentYear = DateTime.Now.Year;
            return View();
        }

        /// <summary>
        /// TempData
        /// </summary>
        /// <returns></returns>
        public IActionResult TempDataExample()
        {
            TempData["CurrentDateTime"] = DateTime.Now;
            TempData["CurrentYear"] = DateTime.Now.Year;
            return RedirectToAction("TempDataShow");
        }

        /// <summary>
        /// TempData
        /// </summary>
        /// <returns></returns>
        public IActionResult TempDataShow()
        {
            return View();
        }

        /// <summary>
        /// Session Variable
        /// </summary>
        /// <returns></returns>
        public IActionResult SessionExample()
        {
            HttpContext.Session.SetString("CurrentDateTime", DateTime.Now.ToString());
            HttpContext.Session.SetInt32("CurrentYear", DateTime.Now.Year);

            // Session 儲存複雜類型數據
            Person p = new Person()
            {
                Name = "Yogi",
                Sex = "Female",
                Address = "Earth",
            };
            HttpContext.Session.Set<Person>("MyPersonClass", p);

            return View();
        }

        #region Performing Redirections in Action Methods
        /// <summary>
        /// Redirect
        /// </summary>
        /// <returns></returns>
        public RedirectResult RedirectAction() => Redirect("/List/Search");

        /// <summary>
        /// RedirectPermanent
        /// </summary>
        /// <returns></returns>
        public RedirectResult RedirectPermanentAction() => RedirectPermanent("/List/Search");

        /// <summary>
        /// RedirectToRoute
        /// </summary>
        /// <returns></returns>
        public RedirectToRouteResult RedirectToRouteAction()
        {
            return RedirectToRoute(new { controller = "Admin", action = "Users", ID = "10" });
        }

        /// <summary>
        /// RedirectToRoutePermanent
        /// </summary>
        /// <returns></returns>
        public RedirectToRouteResult RedirectToRoutePermanentAction()
        {
            return RedirectToRoutePermanent(new { controller = "Admin", action = "Users", ID = "10" });
        }
        #endregion

        #region Returning Different Types of Content from Action Methods
        /// <summary>
        /// Action method returning JSON
        /// </summary>
        /// <returns></returns>
        public JsonResult ReturnJson()
        {
            return Json(new[] { "Brahma", "Vishnu", "Mahesh" });
        }

        /// <summary>
        /// Returning OK (HTTP Status Code 200) from Action
        /// </summary>
        /// <returns></returns>
        public OkObjectResult ReturnOk()
        {
            return Ok(new string[] { "Brahma", "Vishnu", "Mahesh" });
        }

        /// <summary>
        /// Example: Returning BadRequest – 400 Status Code
        /// </summary>
        /// <returns></returns>
        public StatusCodeResult ReturnBadRequst()
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        /// <summary>
        /// Example: Returning Unauthorized – 401 Status Code
        /// </summary>
        /// <returns></returns>
        public StatusCodeResult ReturnUnauthorized()
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        /// <summary>
        /// Example: Returning NotFound – 404 Status Code
        /// </summary>
        /// <returns></returns>
        public StatusCodeResult ReturnNotFound()
        {
            return StatusCode(StatusCodes.Status404NotFound);
            //or return NotFound();
        }
        #endregion
    }
}
