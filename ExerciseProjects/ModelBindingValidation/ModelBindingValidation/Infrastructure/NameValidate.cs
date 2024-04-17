using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ModelBindingValidation.Infrastructure
{
    public class NameValidate : Attribute, IModelValidator
    {
        public string[]? NotAllowed { get; set; }
        public string? ErrorMessage { get; set; }

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            if ((NotAllowed ?? new string[1]).Contains(context.Model as string))
            {
                return new List<ModelValidationResult>
                {
                    new ModelValidationResult("", ErrorMessage),
                };
            }
            else
            {
                return Enumerable.Empty<ModelValidationResult>();
            }
        }
    }
}
