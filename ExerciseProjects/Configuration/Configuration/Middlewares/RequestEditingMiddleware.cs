namespace Configuration.Middlewares
{
    /// <summary>
    /// Request-Editing Middleware
    /// </summary>
    public class RequestEditingMiddleware
    {
        private RequestDelegate nextDelegate;

        public RequestEditingMiddleware(RequestDelegate next) => nextDelegate = next;

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Items["Firefox"] = httpContext.Request.Headers["User-Agent"].Any(x => x.Contains("Firefox"));
            // 測試 Response-Editing Middleware 用
            //httpContext.Items["Firefox"] = true;
            await nextDelegate.Invoke(httpContext);
        }
    }
}
