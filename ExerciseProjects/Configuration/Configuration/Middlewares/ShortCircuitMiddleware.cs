using Configuration.Services;

namespace Configuration.Middlewares
{
    /// <summary>
    /// Short-Circuiting Middleware
    /// </summary>
    public class ShortCircuitMiddleware
    {
        private RequestDelegate nextDelegate;

        public ShortCircuitMiddleware(RequestDelegate next) => nextDelegate = next;

        public async Task Invoke(HttpContext httpContext)
        {
            // httpContext.Items["Firefox"] as bool? == true
            //if (httpContext.Request.Headers["User-Agent"].Any(x => x.Contains("Firefox")))
            if ((httpContext.Items["Firefox"] as bool?) == true)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                await nextDelegate.Invoke(httpContext);
            }
        }
    }
}
