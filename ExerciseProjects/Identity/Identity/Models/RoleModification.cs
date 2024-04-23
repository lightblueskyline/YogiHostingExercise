using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    /// <summary>
    /// 變更角色
    /// </summary>
    public class RoleModification
    {
        [Required]
        public string? RoleName { get; set; }

        public string? RoleId { get; set; }

        public string[]? AddIds { get; set; }

        public string[]? DeleteIds { get; set; }
    }
}
