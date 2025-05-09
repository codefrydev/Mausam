using System.Net.Http.Json;
using Mausam.Models;

namespace Mausam.Services;

public class Nominatim(HttpClient http) : INominatim
{
    public async Task<List<LocationSuggestion>> SearchWithName(string query, int limit = 5)
    {
        return await http.GetFromJsonAsync<List<LocationSuggestion>>(
                   $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(query)}&limit={limit}") ??
               [];
    }

    public async Task<ReverseGeocodeResponse?> ReverseGeocode(GeolocationPosition position)
    {
        return await http.GetFromJsonAsync<ReverseGeocodeResponse>(
            $"https://nominatim.openstreetmap.org/reverse?format=json&lat={position.Coords.Latitude}&lon={position.Coords.Longitude}");
    }
}