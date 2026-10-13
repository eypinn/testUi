using System.Net.Http.Json;
using CityViewerWeb.Models;

namespace CityViewerWeb.Services;

public sealed class CityApiClient(HttpClient httpClient)
{
    public async Task<List<City>> GetCitiesAsync(CancellationToken ct) =>
        await httpClient.GetFromJsonAsync<List<City>>("api/cities", ct) ?? [];
}





