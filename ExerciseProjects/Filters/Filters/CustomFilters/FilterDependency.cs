using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Filters.CustomFilters
{
    /// <summary>
    /// ASP.NET Core Filters Dependency Injection
    /// </summary>
    public interface IExceptionFilterMessage
    {
        public IEnumerable<string> Messages { get; }

        void AddMessage(string message);
    }

    /// <summary>
    /// ASP.NET Core Filters Dependency Injection
    /// </summary>
    public class ExceptionFilterMessage : IExceptionFilterMessage
    {
        private List<string> messages = new List<string>();

        public IEnumerable<string> Messages => messages;

        public void AddMessage(string message) => messages.Add(message);
    }

    /// <summary>
    /// ASP.NET Core Filters Dependency Injection
    /// </summary>
    public class CatchErrorMessage : IExceptionFilter
    {
        private IExceptionFilterMessage iExFilter;

        public CatchErrorMessage(IExceptionFilterMessage ex)
        {
            this.iExFilter = ex;
        }

        public void OnException(ExceptionContext context)
        {
            this.iExFilter.AddMessage("Exception Filter is called. ");
            this.iExFilter.AddMessage("Error Message is given below. ");
            this.iExFilter.AddMessage(context.Exception.Message);

            string allMessage = "";
            foreach (var item in this.iExFilter.Messages)
            {
                allMessage += item;
            }

            context.Result = new ViewResult()
            {
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new EmptyModelMetadataProvider(),
                new ModelStateDictionary())
                {
                    Model = allMessage,
                }
            };
        }
    }
}
