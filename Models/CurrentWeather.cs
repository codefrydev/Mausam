namespace Mausam.Models;

public class CurrentWeather
{
    public double Temperature { get; set; }
    public int WeatherCode { get; set; }
    public double WindSpeed { get; set; }
    public double Precipitation { get; set; }
    public double? ApparentTemperature { get; set; }
    public double? Visibility { get; set; }
    public double? RelativeHumidity { get; set; }
    public double? SurfacePressure { get; set; }
    public double? WindDirection { get; set; }
    public double? Rain { get; set; }
    public double? UvIndex { get; set; }
    public AirQuality? AirQuality { get; set; }
}
