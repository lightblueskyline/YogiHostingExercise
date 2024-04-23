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

        public ClaimsController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
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
    }
}
