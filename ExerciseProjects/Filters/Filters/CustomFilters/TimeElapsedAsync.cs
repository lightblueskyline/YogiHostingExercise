using Microsoft.AspNetCore.Mvc.Filters;

using System.Diagnostics;
using System.Text;

namespace Filters.CustomFilters
{
    /// <summary>
    /// ASP.NET Core Action Filters
    /// </summary>
    public class TimeElapsedAsync : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Stopwatch timer = Stopwatch.StartNew();
            await next();
            string result = $"<div>Elapsed time: {timer.Elapsed.TotalMilliseconds} ms</div>";
            byte[] bytes = Encoding.ASCII.GetBytes(result);
            await context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
