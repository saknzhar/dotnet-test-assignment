using ModelContextProtocol.Server;
using System.ComponentModel;
using WeatherMcpServer.Services;

namespace WeatherMcpServer.Tools;

public class WeatherTool
{
    private readonly IWeatherService _weatherService;

    public WeatherTool(IWeatherService weatherService)
    {
        _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
    }

    [McpServerTool]
    [Description("Gets current weather conditions for the specified city.")]
    public async Task<string> GetCurrentWeather(
        [Description("The city name to get weather for")] string city,
        [Description("Optional: Country code (e.g., 'US', 'UK')")] string? countryCode = null)
    {
        try
        {
            var coordinates = await _weatherService.GetCoordinatesAsync(city, countryCode).ConfigureAwait(false);
            if (coordinates == null)
            {
                return $"Could not find coordinates for {city}, {countryCode ?? "null"}.";
            }

            var weather = await _weatherService.GetWeatherAsync(coordinates.Value.lat, coordinates.Value.lon).ConfigureAwait(false);
            return weather ?? "Could not fetch weather.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return "An error occurred while fetching weather.";
        }
    }

    [McpServerTool]
    [Description("Gets weather forecast for the specified city.")]
    public async Task<string> GetWeatherForecast(
        [Description("The city name to get weather forecast for")] string city,
        [Description("Optional: Country code (e.g., 'US', 'UK')")] string? countryCode = null)
    {
        try
        {
            var coordinates = await _weatherService.GetCoordinatesAsync(city, countryCode).ConfigureAwait(false);
            if (coordinates == null)
            {
                return $"Could not find coordinates for {city}, {countryCode ?? "null"}.";
            }

            var forecast = await _weatherService.GetWeatherForecastAsync(coordinates.Value.lat, coordinates.Value.lon).ConfigureAwait(false);
            return forecast ?? "Could not fetch weather forecast.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return "An error occurred while fetching weather forecast.";
        }
    }

    [McpServerTool]
    [Description("Gets weather alerts/warnings for the specified city.")]
    public async Task<string> GetWeatherAlerts(
        [Description("The city name to get weather alerts for")] string city,
        [Description("Optional: Country code (e.g., 'US', 'UK')")] string? countryCode = null)
    {
        try
        {
            var coordinates = await _weatherService.GetCoordinatesAsync(city, countryCode).ConfigureAwait(false);
            if (coordinates == null)
            {
                return $"Could not find coordinates for {city}, {countryCode ?? "null"}.";
            }

            var alerts = await _weatherService.GetWeatherAlertsAsync(coordinates.Value.lat, coordinates.Value.lon).ConfigureAwait(false);
            return alerts ?? "Could not fetch weather alerts.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return "An error occurred while fetching weather alerts.";
        }
    }
}