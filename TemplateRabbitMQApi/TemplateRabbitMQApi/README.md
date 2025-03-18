# **.NET Template - RabbitMQ API**  

A **lightweight template** for building  applications using **RabbitMQ** as the message broker.  

## 🚀 **Tech Stack**  
- **ASP.NET Core Web API** (C#)  
- **SQL Server** (Database)  
- **RabbitMQ** (Message Broker)  

## 📌 **Features**  
This template provides a structured approach for managing:  
✔ **User Management**  
✔ **Authentication & Authorization**  
✔ **Event Publishing & Subscription**  
✔ **Secure Connection to RabbitMQ**  

## 🏗 **Architecture Overview**  
The system follows a **Producer - Consumer** design:  

![Architecture](Docs/Diagrams/Architecture.jpg)  

### **How It Works**  
1️⃣ **Producers (Services)** generate events (e.g., user created, order placed).  
2️⃣ **RabbitMQ (Event Bus)** routes messages to relevant queues and exchanges.  
3️⃣ **Consumers (Subscribers)** process events asynchronously and update external systems or databases.  
4️⃣ **SQL Server** stores application data.  

---

## 📦 **Getting Started**  

### **Prerequisites**  
- .NET 7+  
- Docker (for SQL Server & RabbitMQ)  
- RabbitMQ Management Plugin enabled  

### **1️⃣ Clone the Repository**  
```sh
git clone https://github.com/your-username/TemplateRabbitMqApi.git
cd TemplateRabbitMqApi
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
  "DefaultConnection": "Server=localhost,1433;Database=rabbitmq_api;User Id=sa;Password=YourPassword123;"
}
```  

### **3️⃣ Start RabbitMQ Server**  
Run RabbitMQ via Docker:  
```sh
docker run -d --hostname rabbitmq \
  -p 5672:5672 -p 15672:15672 \
  --name rabbitmq rabbitmq:3-management
```  
> Access RabbitMQ **Management UI**: [http://localhost:15672](http://localhost:15672)  
> **Default Credentials:**  
> - **User:** `guest`  
> - **Password:** `guest`  

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
✅ RabbitMQ is running  
✅ The API is accessible at `http://localhost:5112`  
