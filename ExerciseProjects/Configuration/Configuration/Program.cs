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

            #region ������ע�Է���
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            #endregion

            // ������ע���Զ��x���� TotalUsers
            builder.Services.AddSingleton<TotalUsers>();

            // �xȡ appsettings.json -> ����
            builder.Services.Configure<OpenWeatherMapApi>(builder.Configuration.GetSection("APIEndpoints"));

            var app = builder.Build();

            #region ���� HTTP Ո��ܵ�
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
                // ��Ӯ���̎�����g��
                app.UseExceptionHandler("/Home/Error");
                // ��� HSTS ���g�������� HSTS �^ (HTTP �ϸ��䰲ȫ)
                app.UseHsts();
            }
            // ��� HTTPS �ض������g����ǿ�� HTTP Ո���ض����� HTTPS
            app.UseHttpsRedirection();

            #region �Զ��x���g��
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

            // ����o�B�ļ����g��
            app.UseStaticFiles();
            // ���·�����g�����Ԍ�Ո��ƥ�䵽�K�c
            app.UseRouting();
            // ����ڙ����g��
            app.UseAuthorization();
            // ���·�����g��
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            #endregion

            app.MapGet("/", () => "Hello World! - Configuration");

            app.Run();
        }
    }
}
