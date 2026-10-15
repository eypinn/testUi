namespace CityViewerWeb.Models;

public sealed class CityException
{
    public int CityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CountryCode { get; set; } = "TR";
    public int? Population { get; set; }
    public decimal? AreaKm2 { get; set; }
    public string? Reason { get; set; }
}
