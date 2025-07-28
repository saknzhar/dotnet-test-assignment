This is a test assignment for FastMCP.me - a service for creating and deploying MCP servers.

## Setup and Configuration Instructions

1.  **Prerequisites:**

    *   [.NET SDK](https://dotnet.microsoft.com/en-us/download) (version 8.0 or later)
    *   A suitable IDE or text editor (e.g., Visual Studio Code, Visual Studio). Visual Studio Code is recommended.
    *   Optional: Docker Desktop (if you want to use Docker).

2.  **Clone the Repository:**

    ```bash
    git clone <repository_url>
    cd <project_directory>
    ```

3.  **Restore Dependencies:**

    ```bash
    dotnet restore
    ```

4.  **Configuration:**

    *   The application uses `appsettings.json` for configuration. Modify this file to suit your needs. Key settings include:
        *   `Logging`: Configures logging levels and providers.
        *   `AllowedHosts`: Specifies the allowed hosts for the application.

5.  **Build the Application:**

    ```bash
    dotnet build
    ```

## Example Usage/Demo

1.  **Run the Application:**

    ```bash
    dotnet run
    ```

    This will start the server. You should see output indicating the server is running and listening on a specific port (typically `http://localhost:5000` or `http://localhost:5001`).

2.  **Access Endpoints:**

    *   Open your web browser or use a tool like `curl` or Postman to access the API endpoints.
    *   Example: `http://localhost:5000/weatherforecast` (This is a standard endpoint created by the default .NET template).

## Implementation Approach Documentation

*   **Project Structure:** The project follows a standard .NET web API structure.
*   **Technology Stack:**
    *   .NET 8.0: The core framework.
    *   ASP.NET Core: For building the web API.
    *   JSON: For configuration and data serialization.
*   **Design Patterns:** The project uses basic dependency injection.
*   **Error Handling:** Global exception handling is implemented using middleware.

