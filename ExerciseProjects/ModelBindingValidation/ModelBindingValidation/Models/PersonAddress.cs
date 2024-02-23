using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ModelBindingValidation.Models
{
    public class PersonAddress
    {
        public string City { get; set; } = String.Empty;

        //[BindNever]
        public string Country { get; set; } = String.Empty;
    }
}
