using Mausam.Models;

namespace Mausam.Services;

public interface IOpenMeteo
{
    Task<WeatherResponse?> GetWeather(double lat, double lon);
    Task<AirQuality?> GetAirQuality(double lat, double lon);
}