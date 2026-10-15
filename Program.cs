using CityViewerWeb.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<CityApiClient>(client =>
{
    var baseUrl = builder.Configuration["CityApi:BaseUrl"]
        ?? throw new InvalidOperationException("CityApi:BaseUrl is missing.");
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddHttpClient<CityExceptionApiClient>(client =>
{
    var baseUrl = builder.Configuration["CityApi:BaseUrl"]
        ?? throw new InvalidOperationException("CityApi:BaseUrl is missing.");
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
});

var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
