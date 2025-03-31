# **.NET Template – MongoDB REST API**  

A **production-ready** REST API, with MongoDB template built with **.NET**.  

## 🚀 Tech Stack  
- **ASP.NET Core (C#)**  
- **MongoDB**  
- **Prometheus & Grafana**  
- **Seq for Logging**  

## 📌 Features  
✔ User Management  
✔ Product & Category Management  
✔ Supplier Management  
✔ Image Uploads  
✔ Import (CSV) & Export (PDF, XLSX, CSV)  
✔ JWT Authentication  

## 🏗 Architecture  
![Architecture](Docs/Diagrams/Architecture.jpg)  

## 🛠 Getting Started  
### **Prerequisites**  
- .NET 7+  
- MongoDB  
- Docker (for Grafana & Prometheus)  

### **Installation**  
1️⃣ **Clone the repository:**  
   ```sh
   git clone https://github.com/ortizdavid/Dotnet_Templates.git
   cd TemplateMongoDbApi/TemplateMongoDbApi
   ```  
2️⃣ **Configure the app:**  
   Update `appsettings.json` with your database connection string.  

3️⃣ **Install dependencies:**  
   ```sh
   dotnet restore
   ```  
5️⃣ **Run the API:**  
   ```sh
   dotnet run
   ```  
6️⃣ **Import Postman collections:**  
   API docs are available in `_Api_Collections` inside **Docs**.  

✅ Ensure **Docker, SQL Server, Prometheus, and Grafana** are running.  
✅ API is accessible at `http://127.0.0.1:5050`.  
✅ Test endpoints using **Postman**.  

