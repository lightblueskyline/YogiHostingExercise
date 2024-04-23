using Identity.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Identity.CustomTagHelpers
{
    [HtmlTargetElement("td", Attributes = "i-role")]
    public class RoleUsersTH : TagHelper
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleUsersTH(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HtmlAttributeName("i-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            IdentityRole? role = await this.roleManager.FindByIdAsync(Role);
            if (role != null)
            {
                foreach (var user in this.userManager.Users)
                {
                    if (user != null && await this.userManager.IsInRoleAsync(user, (role.Name ?? "")))
                    {
                        names.Add((user.UserName ?? ""));
                    }
                }
            }
            output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(",", names));
        }
    }
}
