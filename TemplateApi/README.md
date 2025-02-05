# ASP.NET Core REST API

This repository contains an example of a complete REST API built with ASP.NET Core. It includes features such as CRUD operations, image upload, CSV import/export, PDF and XLSX export, and JWT authentication.

## Tools Used

- **C#**
- **ASP.NET Core**
- **PostgreSQL**

## Problem Statement

The project focuses on managing products, providing a comprehensive set of features for product management.

## Features

- [x] CRUD Operations
- [x] Upload Image
- [x] Import CSV
- [x] Export PDF, XLSX, and CSV
- [x] Authentication (JWT)

## Installation

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Postman](https://www.postman.com/downloads/)

### Setup

1. **Set up the database:**

    Open PostgreSQL and run the database script located in the [Database](TemplateApi/Database) folder.

2. **Configure the application:**

    Update the database connection string in the [appsettings.json](TemplateApi/appsettings.json).

3. **Clone the repository:**

  	```sh
	git clone https://github.com/ortizdavid/TemplateApi.git
	cd TemplateApi/TemplateApi
	```

4. **Install dependencies:**

    Restore the .NET dependencies:

    ```sh
	dotnet restore
	```

5. **Run the project:**

    ```sh
	dotnet run
	```

6. **Import Postman collections:**

    API documentation can be found in the Postman collections included in the [_Api_Collections](TemplateApi/_Api_Collections) folder. These collections provide detailed information on available endpoints, request/response formats, and example usage.

7. **Test the endpoints:**

    Use Postman to test the API endpoints.
