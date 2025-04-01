# 📌 .NET Simple API

A lightweight and production-ready .NET Simple MongoDB API template with built-in features like:

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
git clone https://github.com/ortizdavid/TemplateSimpleMongoDbApi.git
cd TemplateSimpleMongoDbApi
```


### 2. **Update the database connection string** in `appsettings.json`:
```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://127.0.0.1:27017/?tls=false",
    "DatabaseName": "dotnet_template_simple_mongodb_api"
  }
}
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


