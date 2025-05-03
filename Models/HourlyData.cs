using System.Text.Json.Serialization;

namespace Mausam.Models;

public class HourlyData
{
    [JsonPropertyName("time")] public List<string> Time { get; set; }
    [JsonPropertyName("temperature_2m")] public List<double> Temperature2m { get; set; }
}