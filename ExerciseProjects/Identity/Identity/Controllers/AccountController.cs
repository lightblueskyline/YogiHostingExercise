using Identity.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signInMgr)
        {
            this.userManager = userMgr;
            this.signInManager = signInMgr;
        }

        #region Login
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                AppUser? appUser = await this.userManager.FindByEmailAsync((login.Email ?? ""));
                if (appUser != null)
                {
                    await this.signInManager.SignOutAsync();
                    // ASP.NET Core Identity Remember Me
                    Microsoft.AspNetCore.Identity.SignInResult result = await this.signInManager.PasswordSignInAsync(appUser, (login.Password ?? ""), login.Remember, false);
                    if (result.Succeeded)
                    {
                        return Redirect((login.ReturnUrl ?? "/"));
                    }
                }
                ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or password");
            }
            return View(login);
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region ASP.NET Core Identity Role based Authentication
        public IActionResult AccessDenied() => View();
        #endregion
    }
}
