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

    [JsonPropertyName("precipitation_probability_max")]
    public List<double>? PrecipitationProbabilityMax { get; set; }

    [JsonPropertyName("precipitation_sum")]
    public List<double>? PrecipitationSum { get; set; }

    [JsonPropertyName("wind_speed_10m_max")]
    public List<double>? WindSpeed10mMax { get; set; }

    [JsonPropertyName("uv_index_max")]
    public List<double>? UvIndexMax { get; set; }
}