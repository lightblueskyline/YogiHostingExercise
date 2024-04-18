using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Diagnostics;

namespace Filters.CustomFilters
{
    /// <summary>
    /// ASP.NET Core Action Filters
    /// </summary>
    public class TimeElapsed : Attribute, IActionFilter
    {
        private Stopwatch? timer;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            timer?.Stop();

            string result = $" Elapsed time: {timer?.Elapsed.TotalMilliseconds} ms";
            IActionResult? iActionResult = context.Result;
            if (iActionResult != null)
            {
                ((ObjectResult)iActionResult).Value += result;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            timer = Stopwatch.StartNew();
        }
    }
}
