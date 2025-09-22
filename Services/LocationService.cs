using Mausam.Models;
using Microsoft.JSInterop;

namespace Mausam.Services;

public class LocationService : ILocationService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly INominatim _nominatim;

    public LocationService(IJSRuntime jsRuntime, INominatim nominatim)
    {
        _jsRuntime = jsRuntime;
        _nominatim = nominatim;
    }

    public event Action<LocationResult>? LocationRequested;

    public async Task RequestCurrentLocationAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("console.log", "[Mausam] LocationService: Starting location request");
            var position = await _jsRuntime.InvokeAsync<GeolocationPosition>("getCurrentPosition");
            await _jsRuntime.InvokeVoidAsync("console.log", $"[Mausam] LocationService: Got position {position.Coords.Latitude}, {position.Coords.Longitude}");
            
            var reverseResponse = await _nominatim.ReverseGeocode(position);
            await _jsRuntime.InvokeVoidAsync("console.log", $"[Mausam] LocationService: Got reverse geocode {reverseResponse?.DisplayName}");

            var displayName = reverseResponse?.DisplayName ??
                              $"{position.Coords.Latitude:F2}, {position.Coords.Longitude:F2}";

            var result = new LocationResult
            {
                Success = true,
                Latitude = position.Coords.Latitude,
                Longitude = position.Coords.Longitude,
                DisplayName = displayName
            };

            await _jsRuntime.InvokeVoidAsync("console.log", "[Mausam] LocationService: Firing LocationRequested event");
            LocationRequested?.Invoke(result);
        }
        catch (Exception)
        {
            var result = new LocationResult
            {
                Success = false,
                ErrorMessage = "Location access denied or failed"
            };

            LocationRequested?.Invoke(result);
        }
    }
}
