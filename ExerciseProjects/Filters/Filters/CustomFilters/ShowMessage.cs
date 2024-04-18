using Microsoft.AspNetCore.Mvc.Filters;

using System.Text;

namespace Filters.CustomFilters
{
    public class ShowMessage : Attribute, IResultFilter, IOrderedFilter
    {
        public int Order { get; set; }

        private string? message;

        public ShowMessage(string msg)
        {
            this.message = msg;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        { }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            WriteMessage(context, (this.message ?? ""));
        }

        private void WriteMessage(FilterContext context, string msg)
        {
            byte[] bytes = Encoding.ASCII.GetBytes($"<div>{msg}</div>");
            context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
