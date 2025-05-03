using System.Text.Json.Serialization;

namespace Mausam.Models;

public class LocationSuggestion
{
    [JsonPropertyName("display_name")] public string DisplayName { get; set; }
    [JsonPropertyName("lat")] public double Lat { get; set; }
    [JsonPropertyName("lon")] public double Lon { get; set; }
}