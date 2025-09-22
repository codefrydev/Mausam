using System.Text.Json.Serialization;

namespace Mausam.Models;

public class HourlyData
{
    [JsonPropertyName("time")] public List<string> Time { get; set; }
    [JsonPropertyName("temperature_2m")] public List<double> Temperature2m { get; set; }
    [JsonPropertyName("weather_code")] public List<int> WeatherCode { get; set; }
    [JsonPropertyName("precipitation")] public List<double> Precipitation { get; set; }
    [JsonPropertyName("wind_speed_10m")] public List<double> WindSpeed10m { get; set; }
    [JsonPropertyName("visibility")] public List<double> Visibility { get; set; }
    [JsonPropertyName("relative_humidity_2m")] public List<double> RelativeHumidity2m { get; set; }
    [JsonPropertyName("uv_index")] public List<double> UvIndex { get; set; }
}