namespace CityViewerWeb.Models;

public sealed record City(int Id, string Name, string CountryCode, int? Population, double? AreaKm2);
