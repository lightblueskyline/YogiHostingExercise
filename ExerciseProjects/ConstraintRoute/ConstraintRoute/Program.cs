using ConstraintRoute.CustomConstraint;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Custom Routing Constraint
builder.Services.Configure<RouteOptions>(options => options.ConstraintMap.Add("AllowedGods", typeof(OnlyGodsConstraint)));
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Int Routing Constraint
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id:int}");
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id:int?}");

// Range Routing Constraint
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id:range(5,20)?}");

// Regex Routing Constraint
//app.MapControllerRoute(
//    name: "regexConstraint",
//    pattern: "{controller:regex(^H.*)=Home}/{action=Index}/{id?}");
//app.MapControllerRoute(
//    name: "regexConstraint",
//    pattern: "{controller:regex(^H.*)=Home}/{action:regex(^Index$|^About$)=Index}/{id?}");

// Combining Routing Constraints together
//app.MapControllerRoute(
//    name: "combiningConstraint",
//    pattern: "{controller=Home}/{action=Index}/{id:alpha:regex(^H.*)?}");

// Custom Routing Constraint
app.MapControllerRoute(
    name: "combiningConstraint",
    pattern: "{controller=Home}/{action=Index}/{id:AllowedGods}");

app.Run();
