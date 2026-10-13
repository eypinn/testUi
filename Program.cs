using CityViewerWeb.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<CityApiClient>(client =>
{
    var baseUrl = builder.Configuration["CityApi:BaseUrl"]
        ?? throw new InvalidOperationException("CityApi:BaseUrl is missing.");
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
});

var app = builder.Build();
app.UseStaticFiles();
app.MapRazorPages();
app.Run();
