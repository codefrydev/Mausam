namespace Mausam.Models;

public class DailyForecast
{
    public DateTime Date { get; set; }
    public int WeatherCode { get; set; }
    public double MaxTemp { get; set; }
    public double MinTemp { get; set; }
}