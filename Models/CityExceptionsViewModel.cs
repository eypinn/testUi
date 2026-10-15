namespace CityViewerWeb.Models;

public sealed class CityExceptionsViewModel
{
    public List<CityException> Exceptions { get; set; } = [];
    public List<City> Cities { get; set; } = [];
    public CityExceptionEditModel Edit { get; set; } = new();
    public string? Error { get; set; }
    public string? Success { get; set; }
}
