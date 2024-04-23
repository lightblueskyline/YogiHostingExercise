using Identity.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace Identity.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public ViewResult Index() => View(this.roleManager.Roles);

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        #region Create
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await this.roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Errors(result);
                }
            }
            return View(name);
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole? role = await this.roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await this.roleManager.DeleteAsync(role);
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
                ModelState.AddModelError("", "No role found");
            }
            return View("Index", this.roleManager.Roles);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(string id)
        {
            IdentityRole? role = await this.roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            foreach (AppUser user in this.userManager.Users)
            {
                var list = await this.userManager.IsInRoleAsync(user, (role?.Name ?? "")) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEdit
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoleModification model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                #region 添加
                foreach (string userId in (model.AddIds ?? new string[] { }))
                {
                    AppUser? user = await this.userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await this.userManager.AddToRoleAsync(user, (model.RoleName ?? ""));
                        if (!result.Succeeded)
                        {
                            Errors(result);
                        }
                    }
                }
                #endregion

                #region 刪除
                foreach (string userId in (model.DeleteIds ?? new string[] { }))
                {
                    AppUser? user = await this.userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await this.userManager.RemoveFromRoleAsync(user, (model.RoleName ?? ""));
                        if (!result.Succeeded)
                        {
                            Errors(result);
                        }
                    }
                }
                #endregion
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return await Update((model.RoleId ?? ""));
            }
        }
        #endregion
    }
}
