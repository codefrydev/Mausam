using System.Text.Json.Serialization;

namespace Mausam.Models;

public class CurrentWeatherData
{
    [JsonPropertyName("temperature_2m")] public double Temperature2m { get; set; }
    [JsonPropertyName("weather_code")] public int WeatherCode { get; set; }
    [JsonPropertyName("wind_speed_10m")] public double WindSpeed10m { get; set; }
    [JsonPropertyName("precipitation")] public double Precipitation { get; set; }
}