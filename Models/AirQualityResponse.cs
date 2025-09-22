using System.Text.Json.Serialization;

namespace Mausam.Models;

public class AirQualityResponse
{
    [JsonPropertyName("current")] public AirQuality? Current { get; set; }
}
