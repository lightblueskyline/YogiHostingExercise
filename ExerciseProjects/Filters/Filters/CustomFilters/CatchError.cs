﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Filters.CustomFilters
{
    /// <summary>
    /// ASP.NET Core Exception Filters
    /// </summary>
    public class CatchError : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ViewResult()
            {
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new EmptyModelMetadataProvider(),
                new ModelStateDictionary())
                {
                    Model = context.Exception.Message,
                },
            };
        }
    }
}
