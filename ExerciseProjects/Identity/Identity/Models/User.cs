﻿using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class User
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        #region Custom User Properties
        public Country Country { get; set; }

        public int Age { get; set; }

        [Required]
        public string? Salary { get; set; }
        #endregion
    }
}
