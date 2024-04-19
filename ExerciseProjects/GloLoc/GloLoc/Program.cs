using GloLoc.Models;

using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

using System.Globalization;

namespace GloLoc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configuring Globalization & Localization in the Program class 1.Add MVC View Localization Services
            builder.Services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            // Configuring Globalization & Localization in the Program class 2.RequestLocalizationOptions
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                // https://learn.microsoft.com/zh-cn/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-CN"),
                    new CultureInfo("zh-TW")
                };
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                #region Custom Request Culture Provider
                //options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
                //{
                //    var currentCulture = "en-US";
                //    var segments = (context?.Request?.Path ?? new PathString()).Value?.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                //    if (segments?.Length >= 2)
                //    {
                //        string lastSegment = segments[segments.Length - 1];
                //        currentCulture = lastSegment;
                //    }
                //    var requestCulture = new ProviderCultureResult(currentCulture);
                //    return await Task.FromResult(requestCulture);
                //}));
                #endregion

                #region Fetching stored culture from the database
                //options.AddInitialRequestCultureProvider(new MyCultureProvider());
                #endregion
            });

            #region 變更資源文件位置
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            /**
             * 路徑
             * Resources/Controllers/HomeController.zh-CN.resx 或者 Resources/Controllers.HomeController.zh-CN.resx
             * 其他資源文件路徑類似
             */
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Configuring Globalization & Localization in the Program class 3.localization Middleware
            var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            if (locOptions != null)
            {
                app.UseRequestLocalization(locOptions.Value);
            }

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
