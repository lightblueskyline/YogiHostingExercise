using Microsoft.Extensions.Localization;

using System.ComponentModel.DataAnnotations;

namespace GloLoc.Infrastructure
{
    public class CustomDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var _localizationService = (IStringLocalizer<CustomDate>)validationContext.GetService(typeof(IStringLocalizer<CustomDate>));

            if (value != null)
            {
                if ((DateTime)value > DateTime.Now)
                {
                    return new ValidationResult(_localizationService["Date of Birth cannot be in the future"]);
                }
                else if ((DateTime)value < new DateTime(1980, 1, 1))
                {
                    return new ValidationResult(_localizationService["Date of Birth should not be before 1980"]);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return new ValidationResult(_localizationService["Date of Birth value is not valid"]);
            }
        }
    }
}
