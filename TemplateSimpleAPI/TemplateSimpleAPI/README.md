# 📌 .NET Simple API

A lightweight and production-ready .NET Simple API template with built-in features like:

✅ **Entity Framework Core** for database operations  
✅ **Serilog** for structured logging  
✅ **Prometheus** for metrics and monitoring  
✅ **OpenAPI (Swagger)** for API documentation  

## 🚀 Features
- Simple and clean architecture
- RESTful API endpoints for CRUD operations
- Centralized logging with **Serilog** and **Seq**
- Monitoring with **Prometheus**
- Secure with HTTPS

## 🛠 Setup & Run


### 1. **Clone the repository:**
```sh
git clone https://github.com/ortizdavid/TemplateSimpleAPI.git
cd TemplateSimpleAPI
```


### 2. **Update the database connection string** in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=your_db;User Id=your_user;Password=your_password;"
  }
}
```


### 3. **Run database migrations:**
```sh
dotnet ef database update
```


### 4. **Start the API:**
```sh
dotnet run
```


### 5. **Access API:**
- `GET /api/products` → List all products  
- `POST /api/products` → Add a new product  
- `PUT /api/products/{id}` → Update a product  
- `DELETE /api/products/{id}` → Delete a product  


## 📊 Monitoring & Logging
- **Metrics:** Available at `/metrics` (Prometheus)
- **Logs:** Available in **console** and **Seq (`http://localhost:5059`)**


