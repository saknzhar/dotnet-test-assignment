namespace WeatherMcpServer.Services;

public interface IWeatherService
{
    /// <summary>
    /// Method to get the current weather by coordinates.
    /// </summary>
    /// <param name="lat">Latitude</param>
    /// <param name="lon">Longitude</param>
    /// <returns>A string representing the current weather, or null if an error occurs.</returns>
    Task<string?> GetWeatherAsync(double lat, double lon);

    /// <summary>
    /// Method to get coordinates by city name.
    /// </summary>
    /// <param name="city">City name</param>
    /// <param name="countryCode">Optional country code</param>
    /// <returns>A tuple containing the latitude and longitude, or null if not found.</returns>
    Task<(double lat, double lon)?> GetCoordinatesAsync(string city, string? countryCode = null);

    /// <summary>
    /// Method to get the weather forecast by coordinates.
    /// </summary>
    /// <param name="lat">Latitude</param>
    /// <param name="lon">Longitude</param>
    /// <returns>A string representing the weather forecast.</returns>
    Task<string> GetWeatherForecastAsync(double lat, double lon);

    /// <summary>
    /// Method to get weather alerts by coordinates.
    /// </summary>
    /// <param name="lat">Latitude</param>
    /// <param name="lon">Longitude</param>
    /// <returns>A string representing the weather alerts, or null if an error occurs.</returns>
    Task<string?> GetWeatherAlertsAsync(double lat, double lon);
}