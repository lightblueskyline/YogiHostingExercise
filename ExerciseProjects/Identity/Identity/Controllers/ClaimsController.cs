using Identity.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace Identity.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IAuthorizationService authorizationService;

        public ClaimsController(UserManager<AppUser> userManager,
            IAuthorizationService authorizationService)
        {
            this.userManager = userManager;
            this.authorizationService = authorizationService;
        }

        public ViewResult Index() => View(User?.Claims);

        #region PRIVATE
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        #endregion

        #region Create
        public ViewResult Create() => View();

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create_Post(string claimType, string claimValue)
        {
            AppUser? user = await this.userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                Claim claim = new Claim(claimType, claimValue, ClaimValueTypes.String);
                IdentityResult result = await this.userManager.AddClaimAsync(user, claim);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Errors(result);
                }
            }
            return View();
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string claimValues)
        {
            AppUser? user = await this.userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                string[] claimValuesArray = claimValues.Split(";");
                string claimType = claimValuesArray[0], claimValue = claimValuesArray[1], claimIssuer = claimValuesArray[2];
                Claim? claim = User.Claims.FirstOrDefault(x => x.Type == claimType && x.Value == claimValue && x.Issuer == claimIssuer);
                if (claim != null)
                {
                    IdentityResult result = await this.userManager.RemoveClaimAsync(user, claim);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Errors(result);
                    }
                }
            }
            return View(nameof(Index));
        }
        #endregion

        #region ASP.NET Core Identity Policy Authorization
        [Authorize(Policy = "AspManager")]
        public IActionResult Project() => View(nameof(Index), User?.Claims);
        #endregion

        #region Custom Requirement to an Identity Policy
        [Authorize(Policy = "AllowTom")]
        public ViewResult TomFiels() => View(nameof(Index), User?.Claims);
        #endregion

        #region Apply Policy without [Authorize] attribute
        public async Task<IActionResult> PrivateAccess(string title)
        {
            string[] allowUsers = { "tom", "alice" };
            AuthorizationResult authorized = await this.authorizationService.AuthorizeAsync(User, allowUsers, "PrivateAccess");
            if (authorized.Succeeded)
            {
                return View(nameof(Index), User?.Claims);
            }
            else
            {
                return new ChallengeResult();
            }
        }
        #endregion
    }
}
