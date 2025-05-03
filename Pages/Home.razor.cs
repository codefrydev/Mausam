using Mausam.Models;
using Mausam.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Timer = System.Timers.Timer;

namespace Mausam.Pages;

public partial class Home(INominatim nominatim, IOpenMeteo openMeteo, IJSRuntime jsRuntime)
    : IDisposable
{
    private string _searchQuery =string.Empty;
    private bool _showSuggestions;
    private List<LocationSuggestion> _suggestions = [];

    
    private bool _isLoading;
    private bool _hasError;
    private string _errorMessage = "";

    private WeatherResponse? _weatherResponse;
    private CurrentWeather? _currentWeather;
    private List<DailyForecast> _dailyForecast = [];
    private string _locationName = ""; 
    private readonly Timer _debounceTimer = new Timer(300) ;
    protected override void OnInitialized()
    { 
        _debounceTimer.Elapsed += async (sender, e) => await FetchSuggestions();
        _debounceTimer.AutoReset = false;
    }  
    private async void HandleSearchInput(ChangeEventArgs e)
    {
        _searchQuery = e.Value?.ToString()??string.Empty;
        _debounceTimer.Stop();
        _debounceTimer.Start();
    }
    private async Task RenderChart()
    {
        await InitializeChart(_weatherResponse.Hourly);
    }
    private async Task FetchSuggestions()
    {
        if (string.IsNullOrWhiteSpace(_searchQuery))
        {
            _showSuggestions = false;
            return;
        }

        try
        {
            var response = await nominatim.SearchWithName(_searchQuery); 
            _suggestions = response;
            _showSuggestions = true;
            await InvokeAsync(StateHasChanged);
        }
        catch
        {
            _showSuggestions = false;
        }
    }

    private async Task SelectSuggestion(LocationSuggestion suggestion)
    {
        _searchQuery = suggestion.DisplayName;
        _showSuggestions = false;
        await LoadWeatherData(suggestion.Lat, suggestion.Lon, suggestion.DisplayName);
    }

    private async Task SearchByCity()
    {
        if (string.IsNullOrWhiteSpace(_searchQuery)) return;
        
        _isLoading = true;
        _hasError = false;
        StateHasChanged();

        try
        {
            var response = await nominatim.SearchWithName(_searchQuery); 

            if (response?.Count == 0) throw new Exception("City not found");
            var first = response.First();
            await LoadWeatherData(first.Lat, first.Lon, first.DisplayName);
        }
        catch (Exception ex)
        {
            _hasError = true;
            _errorMessage = ex.Message;
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }

    private async Task GetCurrentLocation()
    {
        _isLoading = true;
        StateHasChanged();

        try
        {
            var position = await jsRuntime.InvokeAsync<GeolocationPosition>("getCurrentPosition");
            var reverseResponse = await nominatim.ReverseGeocode(position); 

            var displayName = reverseResponse?.DisplayName ?? $"{position.Coords.Latitude:F2}, {position.Coords.Longitude:F2}";
            await LoadWeatherData(position.Coords.Latitude, position.Coords.Longitude, displayName);
        }
        catch
        {
            _hasError = true;
            _errorMessage = "Location access denied";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task LoadWeatherData(double lat, double lon, string displayName)
    {
        try
        {
            _weatherResponse = await openMeteo.GetWeather(lat, lon);
            if (_weatherResponse != null)
            {
                _currentWeather = new CurrentWeather
                {
                    Temperature = _weatherResponse.Current.Temperature2m,
                    WeatherCode = _weatherResponse.Current.WeatherCode,
                    WindSpeed = _weatherResponse.Current.WindSpeed10m,
                    Precipitation = _weatherResponse.Current.Precipitation
                };

                _dailyForecast = _weatherResponse.Daily.Time
                    .Select((t, i) => new DailyForecast
                    {
                        Date = DateTime.Parse(t),
                        WeatherCode = _weatherResponse.Daily.WeatherCode[i],
                        MaxTemp = _weatherResponse.Daily.Temperature2mMax[i],
                        MinTemp = _weatherResponse.Daily.Temperature2mMin[i]
                    }).ToList();

                _locationName = displayName;
                await RenderChart();
            }

            _hasError = false;
        }
        catch(Exception exception)
        {
            _hasError = true;
            _errorMessage = exception.Message;
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }
    private async Task InitializeChart(HourlyData hourly)
    {
        var labels = hourly.Time.Take(24).Select(t => DateTime.Parse(t).ToString("HH:mm")).ToArray();
        var data = hourly.Temperature2m.Take(24).ToArray();
        await jsRuntime.InvokeVoidAsync("initializeChart", new { Labels = labels, Data = data });
    }

    private MarkupString GetWeatherIcon(int code)
    {
        var icons = new Dictionary<int, string>
        {
            [0] = "fas fa-sun",
            [1] = "fas fa-cloud-sun",
            [2] = "fas fa-cloud",
            [3] = "fas fa-cloud",
            [45] = "fas fa-smog",
            [48] = "fas fa-smog",
            [51] = "fas fa-cloud-rain",
            [53] = "fas fa-cloud-rain",
            [55] = "fas fa-cloud-rain",
            [61] = "fas fa-cloud-showers-heavy",
            [63] = "fas fa-cloud-showers-heavy",
            [65] = "fas fa-cloud-showers-heavy",
            [71] = "fas fa-snowflake",
            [73] = "fas fa-snowflake",
            [75] = "fas fa-snowflake",
            [77] = "fas fa-snowflake",
            [80] = "fas fa-cloud-showers-heavy",
            [81] = "fas fa-cloud-showers-heavy",
            [82] = "fas fa-cloud-showers-heavy",
            [85] = "fas fa-snowflake",
            [86] = "fas fa-snowflake",
            [95] = "fas fa-bolt",
            [96] = "fas fa-bolt",
            [99] = "fas fa-bolt"
        };

        return new MarkupString($"<i class=\"{icons.GetValueOrDefault(code, "fas fa-question")}\"></i>");
    }

    public void Dispose()
    {
        _debounceTimer?.Dispose();
    }
}