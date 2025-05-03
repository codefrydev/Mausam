using System.Text.Json.Serialization;

namespace Mausam.Models;

public class WeatherResponse
{
    [JsonPropertyName("current")] public CurrentWeatherData Current { get; set; }
    [JsonPropertyName("hourly")] public HourlyData Hourly { get; set; }
    [JsonPropertyName("daily")] public DailyData Daily { get; set; }
}