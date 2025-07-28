using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WeatherMcpServer.Services;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Moq.Protected;
using System.Threading;
using System.Net;

namespace WeatherMcpServer.Tests;

public class WeatherServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly Mock<ILogger<WeatherService>> _mockLogger;
    private readonly WeatherService _weatherService;

    public WeatherServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);

        _mockConfig = new Mock<IConfiguration>();
        _mockLogger = new Mock<ILogger<WeatherService>>();

        // Setup configuration to return a default API key
        _mockConfig.Setup(c => c["OpenWeatherMap:ApiKey"]).Returns("testapikey");

        _weatherService = new WeatherService(httpClient, _mockConfig.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetWeatherAsync_ValidCoordinates_ReturnsWeatherString()
    {
        // Arrange
        double lat = 37.7749;
        double lon = -122.4194;
        string expectedWeather = "Current weather: 20°C, Sunny";

        // Mock the HttpMessageHandler to return a successful response with dummy weather data
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"current\":{\"temp\":20,\"weather\":[{\"description\":\"Sunny\"}]}}")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _weatherService.GetWeatherAsync(lat, lon);

        // Assert
        Assert.Equal(expectedWeather, result);
    }

    [Fact]
    public async Task GetWeatherAsync_InvalidCoordinates_ReturnsNull()
    {
        // Arrange
        double lat = -100;
        double lon = -200;

        // Act
        var result = await _weatherService.GetWeatherAsync(lat, lon);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCoordinatesAsync_ValidCity_ReturnsCoordinates()
    {
        // Arrange
        string city = "London";
        double expectedLat = 51.5074;
        double expectedLon = 0.1278;

        // Mock the HttpMessageHandler to return a successful response with dummy coordinates data
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("[{\"lat\":51.5074,\"lon\":0.1278}]")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _weatherService.GetCoordinatesAsync(city);

        // Assert
        Assert.Equal(expectedLat, result?.lat);
        Assert.Equal(expectedLon, result?.lon);
    }

    [Fact]
    public async Task GetCoordinatesAsync_ValidCityAstana_ReturnsCoordinates()
    {
        // Arrange
        string city = "Astana";
        double expectedLat = 51.1801;
        double expectedLon = 71.4460;

        // Mock the HttpMessageHandler to return a successful response with dummy coordinates data
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("[{\"lat\":51.1801,\"lon\":71.4460}]")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _weatherService.GetCoordinatesAsync(city);

        // Assert
        Assert.Equal(expectedLat, result?.lat);
        Assert.Equal(expectedLon, result?.lon);
    }

    [Fact]
    public async Task GetCoordinatesAsync_InvalidCity_ReturnsNull()
    {
        // Arrange
        string city = "";

        // Act
        var result = await _weatherService.GetCoordinatesAsync(city);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetWeatherForecastAsync_ValidCoordinates_ReturnsForecastString()
    {
        // Arrange
        double lat = 37.7749;
        double lon = -122.4194;
        string expectedForecast = "Weather forecast:\nDate: 2024-01-01 00:00:00, Temp: 15°C, Description: Clear sky\n";

        // Mock the HttpMessageHandler to return a successful response with dummy forecast data
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"list\":[{\"dt_txt\":\"2024-01-01 00:00:00\",\"main\":{\"temp\":15},\"weather\":[{\"description\":\"Clear sky\"}]}]}")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _weatherService.GetWeatherForecastAsync(lat, lon);

        // Assert
        Assert.Equal(expectedForecast, result);
    }

    [Fact]
    public async Task GetWeatherAlertsAsync_ValidCoordinates_ReturnsAlertsString()
    {
        // Arrange
        double lat = 37.7749;
        double lon = -122.4194;
        string expectedAlerts = "Weather Alerts:\nEvent: Hurricane, Description: Take cover!\n";

        // Mock the HttpMessageHandler to return a successful response with dummy alerts data
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"alerts\":[{\"event\":\"Hurricane\",\"description\":\"Take cover!\"}]}")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _weatherService.GetWeatherAlertsAsync(lat, lon);

        // Assert
        Assert.Equal(expectedAlerts, result);
    }

    [Fact]
    public async Task GetWeatherAlertsAsync_NoAlerts_ReturnsNoAlertsMessage()
    {
        // Arrange
        double lat = 37.7749;
        double lon = -122.4194;
        string expectedAlerts = "No weather alerts for this location.";

        // Mock the HttpMessageHandler to return a successful response with no alerts data
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{}")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _weatherService.GetWeatherAlertsAsync(lat, lon);

        // Assert
        Assert.Equal(expectedAlerts, result);
    }
}
