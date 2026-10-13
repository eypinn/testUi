using CityViewerWeb.Models;
using CityViewerWeb.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityViewerWeb.Pages;

public sealed class IndexModel(CityApiClient cityApiClient, ILogger<IndexModel> logger) : PageModel
{
    public List<City> Cities { get; private set; } = [];
    public string? Error { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Cities = await cityApiClient.GetCitiesAsync(ct);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "City API request failed");
            Error = "Şehir API'sine bağlanılamadı. API uygulamasının çalıştığını kontrol edin.";
        }
        catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
        {
            logger.LogError(ex, "City API request timed out");
            Error = "Şehir API'si zaman aşımına uğradı.";
        }
    }
}
