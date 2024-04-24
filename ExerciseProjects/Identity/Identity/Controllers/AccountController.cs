using Identity.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

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

        #region Communicate with Google Cloud Console project
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            string? redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = this.signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo? info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }
            var result = await this.signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { (info?.Principal?.FindFirst(ClaimTypes.Name)?.Value ?? ""), (info?.Principal?.FindFirst(ClaimTypes.Email)?.Value ?? "") };
            if (result.Succeeded)
            {
                return View(userInfo);
            }
            else
            {
                AppUser user = new AppUser
                {
                    Email = info?.Principal?.FindFirst(ClaimTypes.Email)?.Value,
                    UserName = info?.Principal?.FindFirst(ClaimTypes.Email)?.Value,
                };
                IdentityResult identityResult = await this.userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    identityResult = await this.userManager.AddLoginAsync(user, (info ?? new UserLoginInfo("", "", "")));
                    if (identityResult.Succeeded)
                    {
                        await this.signInManager.SignInAsync(user, false);
                        return View(userInfo);
                    }
                }
                return AccessDenied();
            }
        }
        #endregion
    }
}
