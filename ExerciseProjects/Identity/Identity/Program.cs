using Identity.CustomPolicy;
using Identity.IdentifyPolicy;
using Identity.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database Connection String
            //builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
            // Sqlite
            builder.Services.AddDbContext<SqliteAppIdentityDbContext>();
            // Set up ASP.NET Core Identity as a Service
            builder.Services
                .AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<SqliteAppIdentityDbContext>()
                .AddDefaultTokenProviders();

            #region 密碼策略
            builder.Services.Configure<IdentityOptions>(opts =>
            {
                opts.Password.RequiredLength = 8;
                opts.Password.RequireLowercase = true;

                #region ASP.NET Core Identity Username and Email Policy
                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
                #endregion
            });
            #endregion

            #region 注冊：自定義密碼驗證策略
            builder.Services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordPolicy>();
            #endregion

            #region 注冊：自定義 Username and Email 驗證策略
            builder.Services.AddTransient<IUserValidator<AppUser>, CustomUsernameEmailPolicy>();
            #endregion

            #region Changing the Default Login URL in Identity
            //builder.Services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Authenticate/Login");
            #endregion

            #region ASP.NET Core Identity Cookie
            builder.Services.ConfigureApplicationCookie(opts =>
            {
                opts.Cookie.Name = ".AspNetCore.Identity.Application";
                opts.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                opts.SlidingExpiration = true;
            });
            #endregion

            #region 自定義 Access Denied 畫面
            builder.Services.ConfigureApplicationCookie(opts =>
            {
                //opts.AccessDeniedPath = "/Stop/Index";
            });
            #endregion

            #region Create ASP.NET Core Identity Policy 身份策略
            builder.Services.AddAuthorization(opts =>
            {
                opts.AddPolicy("AspManager", policy =>
                {
                    policy.RequireRole("Manager");
                    policy.RequireClaim("Coding-Skill", "ASP.NET Core MVC");
                });
            });
            #endregion

            #region Custom Requirement to an Identity Policy
            builder.Services.AddTransient<IAuthorizationHandler, AllowUsersHandler>();
            builder.Services.AddAuthorization(opts =>
            {
                opts.AddPolicy("AllowTom", policy =>
                {
                    policy.AddRequirements(new AllowUserPolicy("tom", "tom1", "tom2"));
                });
            });
            #endregion

            #region Apply Policy without [Authorize] attribute
            builder.Services.AddTransient<IAuthorizationHandler, AllowPrivateHandler>();
            builder.Services.AddAuthorization(opts =>
            {
                opts.AddPolicy("PrivateAccess", policy =>
                {
                    policy.AddRequirements(new AllowPrivatePolicy());
                });
            });
            #endregion

            #region Communicate with Google Cloud Console project
            builder.Services.AddAuthentication()
                .AddGoogle(opts =>
                {
                    opts.ClientId = "50464872240-uu8btpouov03h86agoiv5n6114jmv39u.apps.googleusercontent.com";
                    opts.ClientSecret = "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";
                    opts.SignInScheme = IdentityConstants.ExternalScheme;
                });
            #endregion

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

            // 鑒權
            app.UseAuthentication();
            // 授權
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
