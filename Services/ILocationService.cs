using Mausam.Models;

namespace Mausam.Services;

public interface ILocationService
{
    event Action<LocationResult>? LocationRequested;
    Task RequestCurrentLocationAsync();
}

public class LocationResult
{
    public bool Success { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string DisplayName { get; set; } = "";
    public string ErrorMessage { get; set; } = "";
}
