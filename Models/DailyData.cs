using System.Text.Json.Serialization;

namespace Mausam.Models;

public class DailyData
{
    [JsonPropertyName("time")] public List<string> Time { get; set; }
    [JsonPropertyName("weather_code")] public List<int> WeatherCode { get; set; }

    [JsonPropertyName("temperature_2m_max")]
    public List<double> Temperature2mMax { get; set; }

    [JsonPropertyName("temperature_2m_min")]
    public List<double> Temperature2mMin { get; set; }
}