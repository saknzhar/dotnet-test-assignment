This is a test assignment for FastMCP.me - a service for creating and deploying MCP servers.

## Setup and Configuration Instructions

1.  **Prerequisites:**

    *   [.NET SDK](https://dotnet.microsoft.com/en-us/download) (version 8.0 or later)
    *   A suitable IDE or text editor (e.g., Visual Studio Code, Visual Studio). Visual Studio Code is recommended.
    *   Optional: Docker Desktop (if you want to use Docker).
    *   
## Running the ASP.NET Core MCP Server Locally

1.  **Get the repository root.**

    ```bash
    # bash/zsh
    REPOSITORY_ROOT=$(git rev-parse --show-toplevel)
    ```

    ```powershell
    # PowerShell
    $REPOSITORY_ROOT = git rev-parse --show-toplevel
    ```

2.  **Run the MCP server app.**

    ```bash
    cd $REPOSITORY_ROOT/todo-list
    dotnet run --
