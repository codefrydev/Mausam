namespace Mausam.Models;

public class DailyForecast
{
    public DateTime Date { get; set; }
    public int WeatherCode { get; set; }
    public double MaxTemp { get; set; }
    public double MinTemp { get; set; }
    public double? PrecipitationProbability { get; set; }
    public double? PrecipitationSum { get; set; }
    public double? MaxWindSpeed { get; set; }
    public double? UvIndexMax { get; set; }
}