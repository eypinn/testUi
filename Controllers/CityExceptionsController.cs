using System.Net.Http;
using CityViewerWeb.Models;
using CityViewerWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityViewerWeb.Controllers;

[Route("exceptions")]
public sealed class CityExceptionsController(CityExceptionApiClient exceptionsClient, CityApiClient citiesClient) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(int? cityId, CancellationToken ct)
    {
        var model = await BuildViewModelAsync(ct);
        if (cityId is > 0)
        {
            var entry = model.Exceptions.FirstOrDefault(x => x.CityId == cityId);
            if (entry is not null)
                model.Edit = new CityExceptionEditModel
                {
                    CityId = entry.CityId, Name = entry.Name, CountryCode = entry.CountryCode,
                    Population = entry.Population, AreaKm2 = entry.AreaKm2, Reason = entry.Reason
                };
            else
            {
                var city = model.Cities.FirstOrDefault(x => x.Id == cityId);
                if (city is not null)
                    model.Edit = new CityExceptionEditModel
                    {
                        CityId = city.Id, Name = city.Name, CountryCode = city.CountryCode,
                        Population = city.Population, AreaKm2 = city.AreaKm2
                    };
            }
        }
        model.Success = TempData["Success"] as string;
        return View(model);
    }

    [HttpPost("save")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save([Bind("CityId,Name,CountryCode,Population,AreaKm2,Reason")] CityExceptionEditModel edit, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var model = await BuildViewModelAsync(ct);
            model.Edit = edit;
            model.Error = "Formdaki alanları kontrol edin.";
            return View("Index", model);
        }
        try
        {
            await exceptionsClient.UpsertAsync(edit, ct);
            TempData["Success"] = "İstisna kaydı kaydedildi.";
            return RedirectToAction(nameof(Index), new { cityId = edit.CityId });
        }
        catch (HttpRequestException)
        {
            var model = await BuildViewModelAsync(ct);
            model.Edit = edit;
            model.Error = "İstisna kaydı API üzerinden kaydedilemedi. Şehir ID'sini ve API bağlantısını kontrol edin.";
            return View("Index", model);
        }
    }

    [HttpPost("delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int cityId, CancellationToken ct)
    {
        if (cityId <= 0) return BadRequest();
        try
        {
            await exceptionsClient.DeleteAsync(cityId, ct);
            TempData["Success"] = "İstisna kaydı silindi; şehir artık ana tablo değerleriyle görüntülenecek.";
        }
        catch (HttpRequestException)
        {
            TempData["Success"] = "Silme işlemi başarısız oldu; API bağlantısını kontrol edin.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task<CityExceptionsViewModel> BuildViewModelAsync(CancellationToken ct)
    {
        var model = new CityExceptionsViewModel();
        try
        {
            model.Exceptions = await exceptionsClient.GetExceptionsAsync(ct);
            // Şehir listesi ana view'dan okunur; istisna kaydı olmayan şehirler de seçilebilir.
            model.Cities = await citiesClient.GetCitiesAsync(ct);
        }
        catch (HttpRequestException)
        {
            model.Error = "Şehir API'sine erişilemedi.";
        }
        return model;
    }
}
