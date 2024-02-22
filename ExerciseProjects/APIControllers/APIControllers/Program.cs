using APIControllers.Models;

var builder = WebApplication.CreateBuilder(args);
// Add CORS to services
builder.Services.AddCors();
// Ìí¼Ó·þ„Õ
builder.Services.AddSingleton<IRepository, Repository>();
// Add services to the container.
builder.Services.AddControllersWithViews().AddXmlDataContractSerializerFormatters();

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

// Include the CORS middleware
app.UseCors(builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
