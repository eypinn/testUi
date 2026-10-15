using System.Net;
using System.Net.Http.Json;
using CityViewerWeb.Models;

namespace CityViewerWeb.Services;

public sealed class CityExceptionApiClient(HttpClient httpClient)
{
    public async Task<List<CityException>> GetExceptionsAsync(CancellationToken ct) =>
        await httpClient.GetFromJsonAsync<List<CityException>>("api/city-exceptions", ct) ?? [];

    public async Task<CityException?> GetExceptionAsync(int cityId, CancellationToken ct)
    {
        using var response = await httpClient.GetAsync($"api/city-exceptions/{cityId}", ct);
        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CityException>(cancellationToken: ct);
    }

    public async Task UpsertAsync(CityExceptionEditModel model, CancellationToken ct)
    {
        using var response = await httpClient.PutAsJsonAsync($"api/city-exceptions/{model.CityId}", new
        {
            model.Name, model.CountryCode, model.Population, model.AreaKm2, model.Reason
        }, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int cityId, CancellationToken ct)
    {
        using var response = await httpClient.DeleteAsync($"api/city-exceptions/{cityId}", ct);
        response.EnsureSuccessStatusCode();
    }
}
