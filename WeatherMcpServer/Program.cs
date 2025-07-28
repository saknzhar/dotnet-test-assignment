using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherMcpServer.Services;
using WeatherMcpServer.Tools;

var builder = Host.CreateApplicationBuilder(args);

// Configure all logs to go to stderr (stdout is used for the MCP protocol messages).
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

// Add configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddHttpClient<WeatherService>(); // This will handle HttpClient injection

// Register WeatherTool as a singleton if it’s used across requests (adjust according to your needs)
builder.Services.AddSingleton<WeatherTool>();

// Register WeatherService as transient (or scoped if needed)
builder.Services.AddTransient<IWeatherService, WeatherService>();

// Add MCP server tools
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

// Run the application
await builder.Build().RunAsync();
