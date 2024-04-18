using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.Diagnostics;

namespace Filters.CustomFilters
{
    /// <summary>
    /// Hybrid Action/Result Filter
    /// </summary>
    public class HybridActRes : ActionFilterAttribute
    {
        private Stopwatch? timer;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            timer = Stopwatch.StartNew();
            await next();
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            timer?.Stop();
            context.Result = new ViewResult
            {
                ViewName = "ShowTime",
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = $"Elapsed time: {timer?.Elapsed.TotalMilliseconds} ms",
                }
            };
            await next();
        }
    }
}
