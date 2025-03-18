# Template Simple MVC 

## Overview
This is a basic ASP.NET Core MVC template with essential features such as authentication, session management, logging, and database integration.

## Features
- ASP.NET Core MVC framework
- Entity Framework Core with SQL Server
- User authentication with session management
- Logging with Serilog and Seq
- Metrics monitoring with Prometheus
- Custom middleware integration

## Technologies Used
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Serilog
- Seq
- Prometheus
- Session Management

## Installation
### Prerequisites
- .NET SDK installed
- SQL Server database

### Setup
1. Clone the repository:
   ```sh
   git clone <repository-url>
   cd SimpleMVC
   ```
2. Configure the database connection in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=your_server;Database=your_db;User Id=your_user;Password=your_password;"
   }
   ```
3. Run database migrations:
   ```sh
   dotnet ef database update
   ```
4. Build and run the application:
   ```sh
   dotnet run
   ```


## Usage
### Running the Application
Once the application is running, navigate to:
- `https://localhost:5071/` - Home Page
- `https://localhost:5071/Auth/Login` - Login Page
- `https://localhost:5071/Products` - Product Listing

### Authentication
- Users can log in and manage their session.
- Sessions are managed using `HttpContext.Session`.

### Logging & Monitoring
- Logs are captured with Serilog and can be viewed in Seq (`http://localhost:5059`).
- Prometheus metrics are available at `/metrics`.

## License
This project is open-source. Feel free to modify and use it as needed.

## Contributors
- [Your Name]

