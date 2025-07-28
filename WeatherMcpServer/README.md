# WeatherMcpServer

A .NET MCP (Model Context Protocol) server that provides weather information for specified locations.

## Prerequisites

*   .NET SDK (version 8.0 or later)

## Installation

1.  Clone the repository:

    ```bash
    git clone <repository_url>
    ```

2.  Navigate to the project directory:

    ```bash
    cd dotnet-test-assignment/WeatherMcpServer
    ```

3.  Build the project:

    ```bash
    dotnet build
    ```

4.  Configure the OpenWeatherMap API key:

    *   Create an account at [OpenWeatherMap](https://openweathermap.org/) and obtain an API key.
    *   Add the API key to the `appsettings.json` file:

        ```json
        {
          "Logging": {
            "LogLevel": {
              "Default": "Information",
              "Microsoft.AspNetCore": "Warning"
            }
          },
          "AllowedHosts": "*",
          "OpenWeatherMap": {
            "ApiKey": "<your_api_key>"
          }
        }
        ```

## Usage

The WeatherMcpServer provides the following tools:

*   **GetCurrentWeather:** Gets current weather conditions for the specified city.
    *   Parameters:
        *   `city`: The city name to get weather for (string).
        *   `countryCode` (optional): Country code (e.g., 'US', 'UK') (string).
    *   Example:
        ```json
        {
          "tool": "GetCurrentWeather",
          "args": {
            "city": "London",
            "countryCode": "UK"
          }
        }
        ```
*   **GetWeatherForecast:** Gets weather forecast for the specified city.
    *   Parameters:
        *   `city`: The city name to get weather forecast for (string).
        *   `countryCode` (optional): Country code (e.g., 'US', 'UK') (string).
    *   Example:
        ```json
        {
          "tool": "GetWeatherForecast",
          "args": {
            "city": "London",
            "countryCode": "UK"
          }
        }
        ```
*   **GetWeatherAlerts:** Gets weather alerts/warnings for the specified city.
    *   Parameters:
        *   `city`: The city name to get weather alerts for (string).
        *   `countryCode` (optional): Country code (e.g., 'US', 'UK') (string).
    *   Example:
        ```json
        {
          "tool": "GetWeatherAlerts",
          "args": {
            "city": "Miami",
            "countryCode": "US"
          }
        }
        ```

To use the MCP server, send requests to the standard input stream with the appropriate tool and parameters. The server will respond on the standard output stream. Requests and responses are in JSON format.

## Configuration

The following configuration settings are available:

*   `OpenWeatherMap:ApiKey`: The API key for the OpenWeatherMap API. This is required for the server to function correctly.

## Troubleshooting

*   **API Key Issues:** If you are not getting weather data, ensure that your API key is correctly configured in `appsettings.json` and that it is valid.
*   **Network Issues:** Check your internet connection and ensure that the server can access the OpenWeatherMap API.
*   **Invalid Input:** Ensure that the input parameters are valid and in the correct format.