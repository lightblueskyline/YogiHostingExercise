var builder = WebApplication.CreateBuilder(args);

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

// 路由中間件
app.UseRouting();

app.UseAuthorization();

// 規則特殊的路由，需要靠前配置
// 默認路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action}");
//app.MapControllerRoute(
//    name: "MyRoute",
//    pattern: "{controller=Home}/{action=Index}/{id}");

// ASP.NET Core Static Route
//app.MapControllerRoute(
//    name: "news1",
//    pattern: "News/{controller=Home}/{action=Index}");

// https://localhost:7037/NewsHome/Index
//app.MapControllerRoute(
//    name: "news2",
//    pattern: "News{controller}/{action}");

// Preserving Old Routes
// https://localhost:7244/Shopping/Index -> https://localhost:7244/Home/Index
app.MapControllerRoute(
    name: "shop",
    pattern: "Shopping/{action}",
    defaults: new { controller = "Home" });
app.MapControllerRoute(
    name: "old",
    pattern: "Shopping/Old",
    defaults: new { controller = "Home", action = "Index" });

// “*catchall” for Route Wildcard
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{*catchall}");

app.Run();
