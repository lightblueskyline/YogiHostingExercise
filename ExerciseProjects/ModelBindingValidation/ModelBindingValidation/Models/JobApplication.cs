using Microsoft.AspNetCore.Mvc;

using ModelBindingValidation.Infrastructure;

using System.ComponentModel.DataAnnotations;

namespace ModelBindingValidation.Models
{
    public class JobApplication
    {
        [Required]
        [Display(Name = "Job application name")]
        [NameValidate(NotAllowed = new string[] { "Osama Bin Laden", "Saddam Hussain", "Mohammed Gaddafi" }, ErrorMessage = "You cannot apply for the Job")]
        public string? Name { get; set; }

        //[CustomDate]
        [Remote("ValidateDate", "Job")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Please select your sex")]
        public string? Sex { get; set; }

        [Range(0, 6)]
        public string? Experience { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the Terms")]
        public bool TermsAccepted { get; set; }

        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
