using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.CustomFilters
{
    /// <summary>
    /// ASP.NET Core Result Filters
    /// </summary>
    public class ChangeViewAsync : Attribute, IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            context.Result = new ViewResult
            {
                ViewName = "List",
            };
            await next();
        }
    }
}
