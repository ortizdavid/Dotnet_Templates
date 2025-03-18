# **.NET Template - Event-Driven Architecture with NATS**  

A **lightweight template** for building applications using **NATS** as the message broker.  

## 🚀 **Tech Stack**  
- **ASP.NET Core Web API** (C#)  
- **SQL Server** (Database)  
- **NATS** (Message Broker)  

## 📌 **Features**  
This template provides a structured approach for managing:  
✔ **User Management**  
✔ **Authentication & Authorization**  
✔ **Event Publishing & Subscription**  
✔ **Secure Connection to NATS**  

## 🏗 **Architecture Overview**  
The system follows a **Publisher - Subscriber** design:  

![Architecture](Docs/Diagrams/Architecture.jpg)  

### **How It Works**  
1️⃣ **Producers (Services)** generate events (e.g., user created, order placed).  
2️⃣ **NATS (Event Bus)** routes messages to relevant subjects.  
3️⃣ **Consumers (Subscribers)** process events asynchronously and update external systems or databases.  
4️⃣ **SQL Server** stores application data.  

---

## 📦 **Getting Started**  

### **Prerequisites**  
- .NET 7+  
- Docker (for SQL Server & NATS)  
- NATS Server running  

### **1️⃣ Clone the Repository**  
```sh
git clone https://github.com/your-username/TemplateNatsApi.git
cd TemplateNatsApi
```  

### **2️⃣ Configure the Database**  
Ensure **SQL Server** is running. You can either:  
- Use a **local SQL Server instance**  
- Start a **Docker container**:  
  ```sh
  docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourPassword123' \
    -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:latest
  ```  
> **Update the connection string** in `appsettings.json`:  
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=nats_api;User Id=sa;Password=YourPassword123;"
}
```  

### **3️⃣ Start NATS Server**  
Run NATS via Docker:  
```sh
docker run -d --name nats -p 4222:4222 -p 8222:8222 nats:latest
```  
> Access NATS **Monitoring UI**: [http://localhost:8222](http://localhost:8222)  

### **4️⃣ Apply Database Migrations**  
```sh
dotnet ef database update
```  

### **5️⃣ Run the Application**  
```sh
dotnet run
```  

### **6️⃣ Verify Everything is Running**  
✅ SQL Server is running  
✅ NATS Server is running  
✅ The API is accessible at `http://localhost:5112`  
