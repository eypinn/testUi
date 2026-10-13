using CityViewerWeb.Models;
using CityViewerWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityViewerWeb.Controllers;

public sealed class HomeController : Controller
{
    private readonly CityApiClient _cityApiClient;

    public HomeController(CityApiClient cityApiClient)
    {
        _cityApiClient = cityApiClient;
    }

    [HttpGet("/")]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var model = new CityListViewModel();
        try
        {
            model.Cities = await _cityApiClient.GetCitiesAsync(ct);
        }
        catch (HttpRequestException)
        {
            model.Error = "Şehir API'sine erişilemedi. Önce CityProviderApi uygulamasını başlatın.";
        }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested)
        {
            model.Error = "Şehir API'si zaman aşımına uğradı.";
        }

        return View(model);
    }
}
