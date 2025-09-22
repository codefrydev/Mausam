using Microsoft.AspNetCore.Components;

namespace Mausam.Utils;

public static class WeatherUtils
{
    public static MarkupString GetWeatherIcon(int code)
    {
        var icons = new Dictionary<int, string>
        {
            [0] = "fas fa-sun text-yellow-500",
            [1] = "fas fa-cloud-sun text-yellow-400",
            [2] = "fas fa-cloud text-slate-400",
            [3] = "fas fa-cloud text-slate-500",
            [45] = "fas fa-smog text-slate-400",
            [48] = "fas fa-smog text-slate-500",
            [51] = "fas fa-cloud-rain text-blue-400",
            [53] = "fas fa-cloud-rain text-blue-500",
            [55] = "fas fa-cloud-rain text-blue-600",
            [61] = "fas fa-cloud-showers-heavy text-blue-500",
            [63] = "fas fa-cloud-showers-heavy text-blue-600",
            [65] = "fas fa-cloud-showers-heavy text-blue-700",
            [71] = "fas fa-snowflake text-blue-200",
            [73] = "fas fa-snowflake text-blue-300",
            [75] = "fas fa-snowflake text-blue-400",
            [77] = "fas fa-snowflake text-blue-500",
            [80] = "fas fa-cloud-showers-heavy text-blue-500",
            [81] = "fas fa-cloud-showers-heavy text-blue-600",
            [82] = "fas fa-cloud-showers-heavy text-blue-700",
            [85] = "fas fa-snowflake text-blue-400",
            [86] = "fas fa-snowflake text-blue-500",
            [95] = "fas fa-bolt text-yellow-500",
            [96] = "fas fa-bolt text-yellow-600",
            [99] = "fas fa-bolt text-yellow-700"
        };

        return new MarkupString($"<i class=\"{icons.GetValueOrDefault(code, "fas fa-question text-slate-400")}\"></i>");
    }

    public static string GetWeatherDescription(int code)
    {
        var descriptions = new Dictionary<int, string>
        {
            [0] = "Clear sky",
            [1] = "Mainly clear",
            [2] = "Partly cloudy",
            [3] = "Overcast",
            [45] = "Fog",
            [48] = "Depositing rime fog",
            [51] = "Light drizzle",
            [53] = "Moderate drizzle",
            [55] = "Dense drizzle",
            [61] = "Slight rain",
            [63] = "Moderate rain",
            [65] = "Heavy rain",
            [71] = "Slight snow",
            [73] = "Moderate snow",
            [75] = "Heavy snow",
            [77] = "Snow grains",
            [80] = "Slight rain showers",
            [81] = "Moderate rain showers",
            [82] = "Violent rain showers",
            [85] = "Slight snow showers",
            [86] = "Heavy snow showers",
            [95] = "Thunderstorm",
            [96] = "Thunderstorm with slight hail",
            [99] = "Thunderstorm with heavy hail"
        };

        return descriptions.GetValueOrDefault(code, "Unknown");
    }
}
