using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelpers.CustomTagHelpers
{
    [HtmlTargetElement(Attributes = "action-name")]
    public class SuppressOutputTH : TagHelper
    {
        public string ActionName { get; set; } = String.Empty;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var actionName = ViewContext?.RouteData.Values["action"]?.ToString();
            if ((actionName != null && actionName.Equals(ActionName)))
            {
                output.SuppressOutput();
            }
        }
    }
}
