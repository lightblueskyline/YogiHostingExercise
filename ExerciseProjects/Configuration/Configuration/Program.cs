using Configuration.Middlewares;
using Configuration.Models;
using Configuration.Services;

namespace Configuration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region 向容器注冊服務
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            #endregion

            // 向容器注冊自定義服務 TotalUsers
            builder.Services.AddSingleton<TotalUsers>();

            // 讀取 appsettings.json -> 對象
            builder.Services.Configure<OpenWeatherMapApi>(builder.Configuration.GetSection("APIEndpoints"));

            var app = builder.Build();

            #region 配置 HTTP 請求管道
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            if (app.Environment.IsStaging())
            {
                // do something
            }
            if (app.Environment.IsProduction())
            {
                // 添加異常處理中間件
                app.UseExceptionHandler("/Home/Error");
                // 添加 HSTS 中間件，啓用 HSTS 頭 (HTTP 严格传输安全)
                app.UseHsts();
            }
            // 添加 HTTPS 重定向中間件，强制 HTTP 請求重定向至 HTTPS
            app.UseHttpsRedirection();

            #region 自定義中間件
            if (Convert.ToBoolean(app.Configuration["Middleware:EnableResponseEditingMiddleware"]))
            {
                app.UseMiddleware<ResponseEditingMiddleware>();
            }
            if (Convert.ToBoolean(app.Configuration["Middleware:EnableRequestEditingMiddleware"]))
            {
                app.UseMiddleware<RequestEditingMiddleware>();
            }
            if (Convert.ToBoolean(app.Configuration["Middleware:EnableShortCircuitMiddleware"]))
            {
                app.UseMiddleware<ShortCircuitMiddleware>();
            }
            if ((app.Configuration.GetSection("Middleware")?.GetValue<bool>("EnableContentMiddleware")).Value)
            {
                app.UseMiddleware<ContentMiddleware>();
            }
            #endregion

            // 添加靜態文件中間件
            app.UseStaticFiles();
            // 添加路由中間件，以將請求匹配到終點
            app.UseRouting();
            // 添加授權中間件
            app.UseAuthorization();
            // 添加路由中間件
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            #endregion

            app.MapGet("/", () => "Hello World! - Configuration");

            app.Run();
        }
    }
}
