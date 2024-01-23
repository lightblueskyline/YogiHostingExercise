using Configuration.Services;

namespace Configuration.Middlewares
{
    /// <summary>
    /// Content-Generating Middleware
    /// </summary>
    public class ContentMiddleware
    {
        private RequestDelegate nextDelegate;
        private TotalUsers totalUsers;

        //public ContentMiddleware(RequestDelegate next) => nextDelegate = next;
        public ContentMiddleware(RequestDelegate next, TotalUsers tu)
        {
            nextDelegate = next;
            // 通過依賴注入，注入服務 TotalUsers
            totalUsers = tu;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.ToString() == "/middleware")
            {
                //await httpContext.Response.WriteAsync("This is from the content middleware");
                await httpContext.Response.WriteAsync($"This is from the content middleware, Total Users: {totalUsers.TUsers()}");
            }
            else
            {
                await nextDelegate.Invoke(httpContext);
            }
        }
    }
}
