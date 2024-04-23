using Microsoft.AspNetCore.Identity;

namespace Identity.Models
{
    /// <summary>
    /// 描述角色、用戶詳情
    /// </summary>
    public class RoleEdit
    {
        public IdentityRole? Role { get; set; }

        public IEnumerable<AppUser>? Members { get; set; }

        public IEnumerable<AppUser>? NonMembers { get; set; }
    }
}
