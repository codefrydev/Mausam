using System.Text.Json.Serialization;

namespace Mausam.Models;

public class ReverseGeocodeResponse
{
    [JsonPropertyName("display_name")] public string DisplayName { get; set; }
}