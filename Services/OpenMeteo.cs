using System.Net.Http.Json;
using Mausam.Models;

namespace Mausam.Services;

public class OpenMeteo(HttpClient http) : IOpenMeteo
{
    public async Task<WeatherResponse?> GetWeather(double lat, double lon)
    {
        return await http.GetFromJsonAsync<WeatherResponse>(
            $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,weather_code,wind_speed_10m,precipitation&hourly=temperature_2m&daily=weather_code,temperature_2m_max,temperature_2m_min&timezone=auto");
    }
}