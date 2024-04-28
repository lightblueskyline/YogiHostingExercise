using Microsoft.AspNetCore.Authentication.Cookies;

namespace CA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ASP.NET Core Cookie Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opts =>
                {
                    opts.LoginPath = "/Home/Login";

                    #region Cookie Authentication Timeout
                    opts.Cookie.Name = ".AspNetCore.Cookies";
                    opts.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    opts.SlidingExpiration = true;
                    #endregion
                });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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

            // ASP.NET Core Cookie Authentication
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Secured}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
