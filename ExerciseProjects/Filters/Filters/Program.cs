using Filters.CustomFilters;

namespace Filters
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // ASP.NET Core Filters Dependency Injection
            builder.Services.AddScoped<IExceptionFilterMessage, ExceptionFilterMessage>();

            #region ASP.NET Core Global Filters
            // 第一步
            //builder.Services.AddScoped<TimeElapsed>();
            // 第二步
            builder.Services.AddMvc().AddMvcOptions(options =>
            {
                //options.Filters.AddService(typeof(TimeElapsed));

                // ASP.NET Core Filters Execution Order
                //options.Filters.Add(new ShowMessage("Global"));
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection(); // 可以注釋此代碼，以理解 [RequireHttps]
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
