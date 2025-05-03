using Mausam.Models;

namespace Mausam.Services;

public interface INominatim
{
    Task<List<LocationSuggestion>> SearchWithName(string query,int limit = 5);
    Task<ReverseGeocodeResponse?> ReverseGeocode(GeolocationPosition position);
}