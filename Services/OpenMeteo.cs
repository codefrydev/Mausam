using System.Net.Http.Json;
using Mausam.Models;

namespace Mausam.Services;

public class OpenMeteoService(HttpClient http) : IOpenMeteo
{
    public async Task<WeatherResponse?> GetWeather(double lat, double lon)
    {
        try
        {
            // API parameters for weather data (air quality is fetched separately)
            var currentParams = "temperature_2m,weather_code,wind_speed_10m,precipitation,apparent_temperature,visibility,relative_humidity_2m,surface_pressure,wind_direction_10m,rain,uv_index";
            var hourlyParams = "temperature_2m,weather_code,precipitation,wind_speed_10m,visibility,relative_humidity_2m,uv_index";
            var dailyParams = "weather_code,temperature_2m_max,temperature_2m_min,precipitation_probability_max,precipitation_sum,wind_speed_10m_max,uv_index_max";
            
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current={currentParams}&hourly={hourlyParams}&daily={dailyParams}&timezone=auto";
            
            return await http.GetFromJsonAsync<WeatherResponse>(url);
        }
        catch (Exception ex)
        {
            // Log the exception if needed
            Console.WriteLine($"Error fetching weather data: {ex.Message}");
            return null;
        }
    }

    public async Task<AirQuality?> GetAirQuality(double lat, double lon)
    {
        try
        {
            var url = $"https://air-quality-api.open-meteo.com/v1/air-quality?latitude={lat}&longitude={lon}&current=european_aqi,pm10,pm2_5,carbon_monoxide,nitrogen_dioxide,sulphur_dioxide,ozone";
            
            var response = await http.GetFromJsonAsync<AirQualityResponse>(url);
            return response?.Current;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching air quality data: {ex.Message}");
            return null;
        }
    }
}