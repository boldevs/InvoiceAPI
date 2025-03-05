# Mini-Invoice-Backend 🧾
An Invoice bookkeeping API built with .NET 9, Clean Architecture, and DDD.

---

## 📌 Overview
Mini-Invoice-Backend is a **.NET 9 API** designed for **invoice bookkeeping**, supporting **authentication, customer & item management, and health checks**.  
Built with **Clean Architecture** and **Domain-Driven Design (DDD)**, it ensures **maintainability and scalability**.

---

## ⚡ Tech Stack
- **ASP.NET Core 9** - Web API framework  
- **Entity Framework Core** - ORM for database interactions  
- **SQL Server** - Relational database  
- **Serilog + Seq** - Advanced logging  
- **MediatR** - Implementing CQRS pattern  
- **AutoMapper** - Object-to-object mapping  
- **FluentValidation** - Request validation  
- **HealthChecks.SqlServer** - Database health monitoring  
- **Scalar** - API documentation  
- **xUnit** - Unit and integration testing  

---
## 📂 Project Structure (Clean Architecture)

- src
  - Application: Business logic and use cases
  - Domain: Core domain models and aggregates
  - Infrastructure: Database and external service implementations
  - SharedKernel: Common utilities and base classes
  - Web.Api: API layer (Controllers, Middleware, Config)
    - Endpoints: API endpoints grouped by feature
    - Middleware: Custom middleware (Logging, Auth, etc.)
    - appsettings.json: Configuration (DB, Logging, etc.)
    - Program.cs: API entry point
  - tests
    - IntegrationTests: Full system tests
    - UnitTests: Isolated business logic tests


---

## 🚀 Features
✔ **Authentication (JWT)**  
✔ **Customer Management**  
✔ **Item Management**  
✔ **Invoice Bookkeeping**  
✔ **Health Checks for SQL Server**  
✔ **Logging & Monitoring with Serilog + Seq**  

---

## 🔑 API Endpoints
Endpoints are organized in **`Web.Api/Endpoints/`**, covering:
- **Auth** – Authentication and JWT handling  
- **Customers** – CRUD operations for customers  
- **Items** – Manage invoice items (products/services)  
- **Invoices** – Invoice creation and management  
- **Health Checks** – Monitor database health  

---

## ⚙️ Database Configuration
To configure the database, update the **connection string** in `Web.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "Database": "Data Source=.; Initial Catalog=invoice-database; Integrated Security=True; TrustServerCertificate=True;"
}
````

⚙ Setup & Run
⚡ Clone the repository
- sh
- Copy
- Edit
- git clone https://github.com/boldevs/Mini-Invoice-Backend.git
- cd Mini-Invoice-Backend
- dotnet run --project src/Web.Api
- For run test : dotnet test




