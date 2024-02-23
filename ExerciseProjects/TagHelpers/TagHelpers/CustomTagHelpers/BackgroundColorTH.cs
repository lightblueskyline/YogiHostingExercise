using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelpers.CustomTagHelpers
{
    //[HtmlTargetElement(Attributes = "background-color")]
    [HtmlTargetElement("button", Attributes = "background-color")]
    [HtmlTargetElement("a", Attributes = "background-color")]
    public class BackgroundColorTH : TagHelper
    {
        public string BackgroundColor { get; set; } = String.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("class", $"btn btn-{BackgroundColor}");
        }
    }
}
