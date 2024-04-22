using Identity.IdentifyPolicy;
using Identity.Models;

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

            #region �ܴa����
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

            #region ע�ԣ��Զ��x�ܴa��C����
            builder.Services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordPolicy>();
            #endregion

            #region ע�ԣ��Զ��x Username and Email ��C����
            builder.Services.AddTransient<IUserValidator<AppUser>, CustomUsernameEmailPolicy>();
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

            // �b��
            app.UseAuthentication();
            // �ڙ�
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Admin}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
