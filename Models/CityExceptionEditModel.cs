using System.ComponentModel.DataAnnotations;

namespace CityViewerWeb.Models;

public sealed class CityExceptionEditModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir şehir ID girin.")]
    public int CityId { get; set; }
    [Required(ErrorMessage = "Şehir adı zorunludur."), StringLength(150)]
    public string Name { get; set; } = string.Empty;
    [Required, StringLength(10)]
    public string CountryCode { get; set; } = "TR";
    [Range(0, int.MaxValue)]
    public int? Population { get; set; }
    [Range(typeof(decimal), "0", "9999999999.99")]
    public decimal? AreaKm2 { get; set; }
    [StringLength(250)]
    public string? Reason { get; set; }
}
