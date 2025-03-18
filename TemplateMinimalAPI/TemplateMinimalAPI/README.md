# 📌 .NET Minimal API Template  

A lightweight, production-ready .NET Minimal API template with built-in features like:  
✅ **Entity Framework Core** for database operations  
✅ **Serilog** for structured logging  
✅ **Prometheus** for metrics and monitoring  
✅ **OpenAPI (Swagger)** for API documentation  

## 🚀 Features  
- Minimal and clean architecture  
- RESTful API endpoints for CRUD operations  
- Centralized logging with **Serilog** and **Seq**  
- Monitoring with **Prometheus**  
- Secure with HTTPS  

## 🛠 Setup & Run  
1. **Clone the repository:**  
   ```sh
   git clone https://github.com/ortizdavid/TemplateMinimalAPI.git
   cd TemplateMinimalAPI
   ```

2. **Update the database connection string** in `appsettings.json`:  
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=your_server;Database=your_db;User Id=your_user;Password=your_password;"
     }
   }
   ```

3. **Run database migrations:**  
   ```sh
   dotnet ef database update
   ```

4. **Start the API:**  
   ```sh
   dotnet run
   ```

5. **Access API:**  
   - `GET /api/products` → List all products  
   - `POST /api/products` → Add a new product  
   - `PUT /api/products/{id}` → Update a product  
   - `DELETE /api/products/{id}` → Delete a product  

## 📊 Monitoring  
- **Metrics:** Available at `/metrics` (Prometheus)  
- **Logs:** Available in **console** and **Seq (`http://localhost:5059`)**  

