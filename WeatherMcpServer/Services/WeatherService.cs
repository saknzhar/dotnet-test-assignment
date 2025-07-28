using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WeatherMcpServer.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(HttpClient httpClient, IConfiguration config, ILogger<WeatherService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _apiKey = config["OpenWeatherMap:ApiKey"] ?? string.Empty;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private async Task<JsonDocument?> CallApiAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonDocument.Parse(jsonString);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "API call failed for URL: {Url}", url);
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error parsing JSON for URL: {Url}", url);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Error}", ex.Message);
            return null;
        }
    }

    public async Task<string?> GetWeatherAsync(double lat, double lon)
    {
        if (lat < -90 || lat > 90 || lon < -180 || lon > 180)
        {
            _logger.LogError("Invalid latitude or longitude: Lat={Lat}, Lon={Lon}", lat, lon);
            return null;
        }

        var url = $"https://api.openweathermap.org/data/3.0/onecall?lat={lat}&lon={lon}&exclude=minutely,hourly,alerts&appid={_apiKey}&units=metric";
        var doc = await CallApiAsync(url);

        if (doc == null) return null;

        try
        {
            var current = doc.RootElement.GetProperty("current");
            var temp = current.GetProperty("temp").GetDecimal();
            var desc = current.GetProperty("weather")[0].GetProperty("description").GetString();
            return $"Current weather: {temp}°C, {desc}";
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Key not found in JSON response");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing weather data: {Error}", ex.Message);
            return null;
        }
        finally
        {
            doc?.Dispose();
        }
    }

    public async Task<(double lat, double lon)?> GetCoordinatesAsync(string city, string? countryCode = null)
    {
        if (string.IsNullOrEmpty(city))
        {
            _logger.LogWarning("City name is null or empty.");
            return null;
        }

        var query = countryCode != null ? $"{city},{countryCode}" : city;
        var geoUrl = $"http://api.openweathermap.org/geo/1.0/direct?q={query}&limit=5&appid={_apiKey}";

        var doc = await CallApiAsync(geoUrl);
        if (doc == null) return null;

        try
        {
            if (doc.RootElement.GetArrayLength() == 0)
            {
                _logger.LogWarning("No coordinates found for city: {City}, countryCode: {CountryCode}", city, countryCode);
                return null;
            }

            JsonElement location = doc.RootElement[0];
            if (!location.TryGetProperty("lat", out JsonElement latElement) || !location.TryGetProperty("lon", out JsonElement lonElement))
            {
                _logger.LogWarning("Lat or Lon missing for city: {City}, countryCode: {CountryCode}", city, countryCode);
                return null;
            }

            double lat = latElement.GetDouble();
            double lon = lonElement.GetDouble();

            return (lat, lon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting coordinates for city: {City}, countryCode: {CountryCode}: {Error}", city, countryCode, ex.Message);
            return null;
        }
        finally
        {
            doc?.Dispose();
        }
    }

    public async Task<string> GetWeatherForecastAsync(double lat, double lon)
    {
        var url = $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&units=metric&appid={_apiKey}";
        var doc = await CallApiAsync(url);

        if (doc == null) return "Could not fetch weather forecast.";

        try
        {
            var forecastList = doc.RootElement.GetProperty("list");
            var forecast = new System.Text.StringBuilder("Weather forecast:\n");

            foreach (var item in forecastList.EnumerateArray())
            {
                var date = item.GetProperty("dt_txt").GetString();
                var temp = item.GetProperty("main").GetProperty("temp").GetDecimal();
                var desc = item.GetProperty("weather")[0].GetProperty("description").GetString();
                forecast.AppendLine($"Date: {date}, Temp: {temp}°C, Description: {desc}");
            }

            return forecast.ToString();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Key not found in JSON response");
            return "Could not parse weather forecast.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing weather forecast data: {Error}", ex.Message);
            return "Could not process weather forecast.";
        }
        finally
        {
            doc?.Dispose();
        }
    }

    public async Task<string?> GetWeatherAlertsAsync(double lat, double lon)
    {
        var url = $"https://api.openweathermap.org/data/3.0/onecall?lat={lat}&lon={lon}&exclude=minutely,hourly,current,daily&appid={_apiKey}&units=metric";
        var doc = await CallApiAsync(url);

        if (doc == null) return "Could not fetch weather alerts.";

        try
        {
            if (!doc.RootElement.TryGetProperty("alerts", out JsonElement alertsElement))
            {
                return "No weather alerts for this location.";
            }

            if (alertsElement.GetArrayLength() == 0)
            {
                return "No weather alerts for this location.";
            }

            var alerts = new System.Text.StringBuilder("Weather Alerts:\n");
            foreach (var alert in alertsElement.EnumerateArray())
            {
                var eventName = alert.GetProperty("event").GetString();
                var description = alert.GetProperty("description").GetString();
                alerts.AppendLine($"Event: {eventName}, Description: {description}");
            }

            return alerts.ToString();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Key not found in JSON response");
            return "Could not parse weather alerts.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing weather alerts data: {Error}", ex.Message);
            return "Could not process weather alerts.";
        }
        finally
        {
            doc?.Dispose();
        }
    }
}