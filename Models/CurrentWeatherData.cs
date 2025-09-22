using System.Text.Json.Serialization;

namespace Mausam.Models;

public class CurrentWeatherData
{
    [JsonPropertyName("temperature_2m")] public double Temperature2m { get; set; }
    [JsonPropertyName("weather_code")] public int WeatherCode { get; set; }
    [JsonPropertyName("wind_speed_10m")] public double WindSpeed10m { get; set; }
    [JsonPropertyName("precipitation")] public double Precipitation { get; set; }
    [JsonPropertyName("apparent_temperature")] public double? ApparentTemperature { get; set; }
    [JsonPropertyName("visibility")] public double? Visibility { get; set; }
    [JsonPropertyName("relative_humidity_2m")] public double? RelativeHumidity2m { get; set; }
    [JsonPropertyName("surface_pressure")] public double? SurfacePressure { get; set; }
    [JsonPropertyName("wind_direction_10m")] public double? WindDirection10m { get; set; }
    [JsonPropertyName("rain")] public double? Rain { get; set; }
    [JsonPropertyName("uv_index")] public double? UvIndex { get; set; }
    [JsonPropertyName("air_quality")] public AirQuality? AirQuality { get; set; }
}
