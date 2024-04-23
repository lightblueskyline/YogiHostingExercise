using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class AppUser : IdentityUser
    {
        #region Custom User Properties
        public Country Country { get; set; }

        public int Age { get; set; }

        [Required]
        public string? Salary { get; set; }
        #endregion
    }

    public enum Country
    {
        USA, UK, France, Germany, Russia, China
    }
}
