using Mausam.Models;
using Mausam.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Timer = System.Timers.Timer;

namespace Mausam.Pages;

public partial class Home(INominatim nominatim, IOpenMeteo openMeteo, ILocationService locationService, IJSRuntime jsRuntime)
    : IDisposable
{
    private string _searchQuery = string.Empty;
    private bool _showSuggestions;
    private List<LocationSuggestion> _suggestions = [];


    private bool _isLoading;
    private bool _hasError;
    private string _errorMessage = "";

    private WeatherResponse? _weatherResponse;
    private CurrentWeather? _currentWeather;
    private List<DailyForecast> _dailyForecast = [];
    private string _locationName = "";
    private readonly Timer _debounceTimer = new(300);

    protected override async Task OnInitializedAsync()
    {
        _debounceTimer.Elapsed += async (sender, e) => await FetchSuggestions();
        _debounceTimer.AutoReset = false;
        
        // Subscribe to location service events
        locationService.LocationRequested += OnLocationRequested;
        _ = jsRuntime.InvokeVoidAsync("console.log", "[Mausam] Event subscription completed");
        _ = jsRuntime.InvokeVoidAsync("console.log", $"[Mausam] LocationService instance: {locationService.GetHashCode()}");
        
        // Load default weather data for a popular location (London)
        _ = jsRuntime.InvokeVoidAsync("console.log", "[Mausam] Loading initial weather data for London, UK");
        await LoadWeatherData(51.5074, -0.1278, "London, UK");
    }

    private void HandleSearchInput(string searchQuery)
    {
        _searchQuery = searchQuery;
        _debounceTimer.Stop();
        _debounceTimer.Start();
    }

    private async Task RenderCharts()
    {
        // Ensure the DOM is updated before initializing the charts
        StateHasChanged();
        await Task.Delay(200); // Increased delay to ensure DOM is fully rendered
        
        if (_weatherResponse?.Hourly != null)
        {
            try
            {
                // Wait for Chart.js to be loaded
                await jsRuntime.InvokeVoidAsync("waitForChartJs");
                
                // Check if Chart.js is loaded
                var chartJsLoaded = await jsRuntime.InvokeAsync<bool>("eval", "typeof Chart !== 'undefined'");
                
                if (chartJsLoaded)
                {
                    Console.WriteLine("Chart.js loaded, initializing charts...");
                    await InitializeTemperatureChart(_weatherResponse.Hourly);
                    await InitializePrecipitationChart(_weatherResponse.Hourly);
                    await InitializeWindChart(_weatherResponse.Hourly);
                    await InitializeHumidityChart(_weatherResponse.Hourly);
                    Console.WriteLine("All charts initialized successfully");
                }
                else
                {
                    Console.WriteLine("Chart.js failed to load after waiting");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RenderCharts: {ex.Message}");
                await jsRuntime.InvokeVoidAsync("console.error", $"RenderCharts error: {ex.Message}");
            }
        }
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
            var first = response?.First() ?? throw new Exception("No location data available");
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

    private void OnLocationRequested(LocationResult result)
    {
        Console.WriteLine($"Location requested: Success={result.Success}, Lat={result.Latitude}, Lon={result.Longitude}, Name={result.DisplayName}");
        _ = jsRuntime.InvokeVoidAsync("console.log", $"[Mausam] Location requested: success={result.Success}, lat={result.Latitude}, lon={result.Longitude}, name={result.DisplayName}");
        
        _ = InvokeAsync(async () =>
        {
            if (result.Success)
            {
                _isLoading = true;
                _hasError = false;
                StateHasChanged();
                _ = jsRuntime.InvokeVoidAsync("console.log", $"[Mausam] Loading weather for {result.DisplayName} ({result.Latitude}, {result.Longitude})");
                await LoadWeatherData(result.Latitude, result.Longitude, result.DisplayName);
                _ = jsRuntime.InvokeVoidAsync("console.log", $"[Mausam] Weather loaded for {result.DisplayName}");
            }
            else
            {
                _hasError = true;
                _errorMessage = result.ErrorMessage;
                _ = jsRuntime.InvokeVoidAsync("console.error", $"[Mausam] Location error: {_errorMessage}");
                StateHasChanged();
            }
        });
    }

    private async Task GetCurrentLocation()
    {
        _isLoading = true;
        StateHasChanged();

        try
        {
            var position = await jsRuntime.InvokeAsync<GeolocationPosition>("getCurrentPosition");
            var reverseResponse = await nominatim.ReverseGeocode(position);

            var displayName = reverseResponse?.DisplayName ??
                              $"{position.Coords.Latitude:F2}, {position.Coords.Longitude:F2}";
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
        Console.WriteLine($"Loading weather data for: {displayName} ({lat}, {lon})");
        try
        {
            _weatherResponse = await openMeteo.GetWeather(lat, lon);
            var airQualityData = await openMeteo.GetAirQuality(lat, lon);
            
            if (_weatherResponse != null)
            {
                _currentWeather = new CurrentWeather
                {
                    Temperature = _weatherResponse.Current.Temperature2m,
                    WeatherCode = _weatherResponse.Current.WeatherCode,
                    WindSpeed = _weatherResponse.Current.WindSpeed10m,
                    Precipitation = _weatherResponse.Current.Precipitation,
                    ApparentTemperature = _weatherResponse.Current.ApparentTemperature,
                    Visibility = _weatherResponse.Current.Visibility,
                    RelativeHumidity = _weatherResponse.Current.RelativeHumidity2m,
                    SurfacePressure = _weatherResponse.Current.SurfacePressure,
                    WindDirection = _weatherResponse.Current.WindDirection10m,
                    Rain = _weatherResponse.Current.Rain,
                    UvIndex = _weatherResponse.Current.UvIndex,
                    AirQuality = airQualityData != null ? new AirQuality
                    {
                        Pm25 = airQualityData.Pm25,
                        Pm10 = airQualityData.Pm10,
                        Ozone = airQualityData.Ozone,
                        NitrogenDioxide = airQualityData.NitrogenDioxide,
                        EuropeanAqi = airQualityData.EuropeanAqi,
                        CarbonMonoxide = airQualityData.CarbonMonoxide,
                        SulphurDioxide = airQualityData.SulphurDioxide
                    } : null
                };

                _dailyForecast = _weatherResponse.Daily.Time
                    .Select((t, i) => new DailyForecast
                    {
                        Date = DateTime.Parse(t),
                        WeatherCode = _weatherResponse.Daily.WeatherCode[i],
                        MaxTemp = _weatherResponse.Daily.Temperature2mMax[i],
                        MinTemp = _weatherResponse.Daily.Temperature2mMin[i],
                        PrecipitationProbability = _weatherResponse.Daily.PrecipitationProbabilityMax?.ElementAtOrDefault(i),
                        PrecipitationSum = _weatherResponse.Daily.PrecipitationSum?.ElementAtOrDefault(i),
                        MaxWindSpeed = _weatherResponse.Daily.WindSpeed10mMax?.ElementAtOrDefault(i),
                        UvIndexMax = _weatherResponse.Daily.UvIndexMax?.ElementAtOrDefault(i)
                    }).ToList();

                _locationName = displayName;
                _ = jsRuntime.InvokeVoidAsync("console.log", $"[Mausam] Location name set to: {_locationName}");
                StateHasChanged(); // Ensure UI updates with new location name
                await RenderCharts();
            }

            _hasError = false;
        }
        catch (Exception exception)
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

    private async Task InitializeTemperatureChart(HourlyData hourly)
    {
        try
        {
            if (hourly?.Time == null || hourly?.Temperature2m == null) 
            {
                Console.WriteLine("Temperature chart: Missing hourly data");
                return;
            }

            var labels = hourly.Time.Take(24).Select(t => DateTime.Parse(t).ToString("HH:mm")).ToArray();
            var data = hourly.Temperature2m.Take(24).ToArray();
            
            Console.WriteLine($"Temperature chart: Initializing with {labels.Length} data points");
            await jsRuntime.InvokeVoidAsync("initializeTemperatureChart", new { Labels = labels, Data = data });
            Console.WriteLine("Temperature chart: Initialized successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing temperature chart: {ex.Message}");
            await jsRuntime.InvokeVoidAsync("console.error", $"Temperature chart error: {ex.Message}");
        }
    }

    private async Task InitializePrecipitationChart(HourlyData hourly)
    {
        try
        {
            if (hourly?.Time == null || hourly?.Precipitation == null) 
            {
                Console.WriteLine("Precipitation chart: Missing hourly data");
                return;
            }

            var labels = hourly.Time.Take(24).Select(t => DateTime.Parse(t).ToString("HH:mm")).ToArray();
            var data = hourly.Precipitation.Take(24).ToArray();
            
            Console.WriteLine($"Precipitation chart: Initializing with {labels.Length} data points");
            await jsRuntime.InvokeVoidAsync("initializePrecipitationChart", new { Labels = labels, Data = data });
            Console.WriteLine("Precipitation chart: Initialized successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing precipitation chart: {ex.Message}");
            await jsRuntime.InvokeVoidAsync("console.error", $"Precipitation chart error: {ex.Message}");
        }
    }

    private async Task InitializeWindChart(HourlyData hourly)
    {
        try
        {
            if (hourly?.Time == null || hourly?.WindSpeed10m == null) 
            {
                Console.WriteLine("Wind chart: Missing hourly data");
                return;
            }

            var labels = hourly.Time.Take(8).Select(t => DateTime.Parse(t).ToString("HH:mm")).ToArray();
            var data = hourly.WindSpeed10m.Take(8).ToArray();
            
            Console.WriteLine($"Wind chart: Initializing with {labels.Length} data points");
            await jsRuntime.InvokeVoidAsync("initializeWindChart", new { Labels = labels, Data = data });
            Console.WriteLine("Wind chart: Initialized successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing wind chart: {ex.Message}");
            await jsRuntime.InvokeVoidAsync("console.error", $"Wind chart error: {ex.Message}");
        }
    }

    private async Task InitializeHumidityChart(HourlyData hourly)
    {
        try
        {
            if (hourly?.RelativeHumidity2m == null) 
            {
                Console.WriteLine("Humidity chart: Missing hourly data");
                return;
            }

            var currentHumidity = hourly.RelativeHumidity2m.FirstOrDefault();
            
            Console.WriteLine($"Humidity chart: Initializing with humidity value: {currentHumidity}");
            await jsRuntime.InvokeVoidAsync("initializeHumidityChart", new { Humidity = currentHumidity });
            Console.WriteLine("Humidity chart: Initialized successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing humidity chart: {ex.Message}");
            await jsRuntime.InvokeVoidAsync("console.error", $"Humidity chart error: {ex.Message}");
        }
    }


    public void Dispose()
    {
        _debounceTimer?.Dispose();
        locationService.LocationRequested -= OnLocationRequested;
    }
}