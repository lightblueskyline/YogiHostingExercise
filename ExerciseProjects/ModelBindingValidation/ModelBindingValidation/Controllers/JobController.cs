using Microsoft.AspNetCore.Mvc;

using ModelBindingValidation.Models;

namespace ModelBindingValidation.Controllers
{
    public class JobController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(JobApplication jobApplication)
        {
            #region ASP.NET Core Server Side Model Validations
            //if (string.IsNullOrEmpty(jobApplication.Name))
            //{
            //    ModelState.AddModelError(nameof(jobApplication.Name), "Please enter your name");
            //}
            //else if (jobApplication.Name == "Osama Bin Laden")
            //{
            //    // Model-Level Error Messages
            //    ModelState.AddModelError("", "You cannot apply for the Job");
            //}
            // 使用 NameValidate
            //if (jobApplication.Name == "Osama Bin Laden")
            //{
            //    // Model-Level Error Messages
            //    ModelState.AddModelError("", "You cannot apply for the Job");
            //}

            // 使用 CustomDate
            //if (jobApplication.DOB == Convert.ToDateTime("01-01-0001 00:00:00"))
            //{
            //    ModelState.AddModelError(nameof(jobApplication.DOB), "Please enter your Date of Birth");
            //}
            //else if (jobApplication.DOB > DateTime.Now)
            //{
            //    ModelState.AddModelError(nameof(jobApplication.DOB), "Date of Birth cannot be in the future");
            //}
            //else if (jobApplication.DOB < new DateTime(1980, 1, 1))
            //{
            //    ModelState.AddModelError(nameof(jobApplication.DOB), "Date of Birth should not be before 1980");
            //}

            //if (string.IsNullOrEmpty(jobApplication.Sex))
            //{
            //    ModelState.AddModelError(nameof(jobApplication.Sex), "Please select your sex");
            //}

            //if (jobApplication?.Experience?.ToString() == "Select")
            //{
            //    ModelState.AddModelError(nameof(jobApplication.Experience), "Please select your experience");
            //}

            //if (!(jobApplication ?? new()).TermsAccepted)
            //{
            //    ModelState.AddModelError(nameof(jobApplication.TermsAccepted), "You must accept the Terms");
            //}
            #endregion

            if (ModelState.IsValid)
            {
                return View("Accepted", jobApplication);
            }
            else
            {
                return View();
            }
        }

        #region ASP.NET Core Remote Validation
        public JsonResult ValidateDate(DateTime DOB)
        {
            if (DOB > DateTime.Now)
            {
                return Json("Date of Birth cannot be in the future");
            }
            else if (DOB < new DateTime(1980, 1, 1))
            {
                return Json("Date of Birth should not be before 1980");
            }
            else
            {
                return Json(true);
            }
        }
        #endregion
    }
}
