namespace CityViewerWeb.Models;

public sealed class CityListViewModel
{
    public List<City> Cities { get; set; } = [];
    public string? Error { get; set; }
}
