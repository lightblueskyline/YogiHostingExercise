using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.CustomFilters
{
    /// <summary>
    /// ASP.NET Core Result Filters
    /// </summary>
    public class ChangeView : Attribute, IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        { }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.Result = new ViewResult
            {
                ViewName = "List"
            };
        }
    }
}
