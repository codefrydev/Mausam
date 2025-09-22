using System.Text.Json.Serialization;

namespace Mausam.Models;

public class AirQuality
{
    [JsonPropertyName("pm2_5")] public double? Pm25 { get; set; }
    [JsonPropertyName("pm10")] public double? Pm10 { get; set; }
    [JsonPropertyName("ozone")] public double? Ozone { get; set; }
    [JsonPropertyName("nitrogen_dioxide")] public double? NitrogenDioxide { get; set; }
    [JsonPropertyName("european_aqi")] public int? EuropeanAqi { get; set; }
    [JsonPropertyName("carbon_monoxide")] public double? CarbonMonoxide { get; set; }
    [JsonPropertyName("sulphur_dioxide")] public double? SulphurDioxide { get; set; }
}
