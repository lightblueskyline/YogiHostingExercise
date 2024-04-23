using Identity.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IPasswordHasher<AppUser> passwordHasher;
        private readonly IPasswordValidator<AppUser> passwordValidator;
        private readonly IUserValidator<AppUser> userValidator;

        public AdminController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> passwordHasher,
            IPasswordValidator<AppUser> passwordValidator, IUserValidator<AppUser> userValidator)
        {
            this.userManager = usrMgr;
            this.passwordHasher = passwordHasher;
            this.passwordValidator = passwordValidator;
            this.userValidator = userValidator;
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        #region Create
        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = user.Name,
                    Email = user.Email,
                    Country = user.Country,
                    Age = user.Age,
                    Salary = user.Salary,
                };
                // Table: AspNetUsers
                IdentityResult result = await userManager.CreateAsync(appUser, (user.Password ?? ""));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(user);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(string id)
        {
            AppUser? user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password,
            int age, string country, string salary)
        {
            // 注意：更新時不會自動應用自定義驗證策略
            // 需要自行添加
            AppUser? user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult? validEmail = null;
                if (!String.IsNullOrEmpty(email))
                {
                    validEmail = await this.userValidator.ValidateAsync(this.userManager, user);
                    if (validEmail.Succeeded)
                    {
                        user.Email = email;
                    }
                    else
                    {
                        Errors(validEmail);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email cannot be empty");
                }

                IdentityResult? validPass = null;
                if (!String.IsNullOrEmpty(password))
                {
                    validPass = await this.passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = this.passwordHasher.HashPassword(user, password);
                    }
                    else
                    {
                        Errors(validPass);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Password cannot be empty");
                }

                user.Age = age;

                Country myCountry;
                Enum.TryParse(country, out myCountry);
                user.Country = myCountry;

                if (!String.IsNullOrEmpty(salary))
                {
                    user.Salary = salary;
                }
                else
                {
                    ModelState.AddModelError("", "Salary cannot be empty");
                }

                if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password) &&
                    (validEmail ?? new IdentityResult()).Succeeded && (validPass ?? new IdentityResult()).Succeeded &&
                    !String.IsNullOrEmpty(salary))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Errors(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View(user);
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser? user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Errors(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index", userManager.Users);
        }
        #endregion

        #region PRIVATE
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        #endregion
    }
}
