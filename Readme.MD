# MiniOrderApp

MiniOrderApp is a full-stack order management system built with ASP.NET Core Web API, Entity Framework Core, SQLite, and modern web technologies (HTML, CSS, JavaScript).

The application follows a clean architecture pattern with separate projects for Domain, Infrastructure, and API.

## Features

- **RESTful Web API** with Swagger/OpenAPI documentation
- **Interactive Web Frontend** with modern UI design
- **Entity Framework Core** for database access with SQLite
- **CRUD operations** for customers, orders, and returns
- **Order management** with multiple items per order
- **Return processing** with reason tracking and refund amounts
- **CORS enabled** for API access

## Architecture

### Projects

- **MiniOrderApp.Domain**: Core business entities and interfaces
- **MiniOrderApp.Infrastructure**: EF Core DbContext and repository implementations
- **MiniOrderApp.Api**: ASP.NET Core Web API with controllers and frontend

### Technology Stack

- .NET 10.0
- ASP.NET Core Web API
- Entity Framework Core 10.0
- SQLite database
- Swashbuckle (Swagger UI)
- Vanilla JavaScript frontend

## Getting Started

### Requirements

- .NET SDK 10.0 or later
- Git (optional, for cloning)

### Build and Run

1. **Clone or download the repository**

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run the Web API**
   ```bash
   dotnet run --project MiniOrderApp.Api
   ```

5. **Access the application**
   - **Web Frontend**: https://localhost:5242/
   - **Swagger UI**: https://localhost:5242/swagger

## API Endpoints

### Customers
- `GET /api/customers` - Get all customers
- `GET /api/customers/{id}` - Get customer by ID
- `POST /api/customers` - Create new customer
- `PUT /api/customers/{id}` - Update customer
- `DELETE /api/customers/{id}` - Delete customer

### Orders
- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `GET /api/orders/{id}/items` - Get order items
- `POST /api/orders` - Create new order
- `PUT /api/orders/{id}/status` - Update order status
- `DELETE /api/orders/{id}` - Delete order

### Returns
- `GET /api/returns` - Get all returns
- `GET /api/returns/order/{orderId}` - Get return by order ID
- `POST /api/returns` - Create new return

## Configuration

The application uses SQLite with a connection string configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "AppDb": "Data Source=orderdata.db"
}
```

The database is automatically created on first run using EF Core migrations.

## What I Learned

Building this project taught me several important software development concepts:

### Architecture & Design Patterns
- **Separation of Concerns**: Organizing code into distinct layers (Domain, Infrastructure, API) where each has a specific responsibility
- **Repository Pattern**: Abstracting database access through repositories, providing a clean interface between the application and data layer
- **Service Layer Pattern**: Using services to orchestrate business logic and coordinate between repositories
- **Clean Architecture**: Keeping the domain layer independent and at the center, with dependencies pointing inward

### Domain-Driven Design
- **Domain Entities**: Creating rich domain models that protect their own invariants and encapsulate business rules
- **Validation in Constructors**: Ensuring entities are always in a valid state by validating data when objects are created
- **Private Setters**: Protecting entity state from unauthorized modifications outside the class
- **Encapsulation**: Using private methods and backing fields to control how entity data is modified

### API Development
- **RESTful Endpoints**: Designing intuitive API routes following REST conventions (GET, POST, PUT, DELETE)
- **Controllers**: Understanding how controllers handle HTTP requests and coordinate between the client and services
- **DTOs (Data Transfer Objects)**: Separating API response models from domain entities to control what data is exposed
- **HTTP Status Codes**: Returning appropriate status codes (200 OK, 201 Created, 404 Not Found, etc.)

### Asynchronous Programming
- **Async/Await Pattern**: Using `async` and `await` keywords for non-blocking I/O operations
- **Task<T>**: Understanding how `Task` represents asynchronous operations that return values
- **Benefits**: Learning that async operations improve scalability by freeing up threads during database/IO operations

### Entity Framework Core
- **Code-First Approach**: Defining database schema through C# classes and migrations
- **DbContext**: Understanding the role of DbContext as the bridge between domain entities and the database
- **Migrations**: Using EF Core migrations to version and evolve the database schema
- **Relationships**: Configuring entity relationships (one-to-many between Order and OrderItem)

### Additional Concepts
- **Dependency Injection**: Registering and injecting services, repositories, and DbContext through the built-in DI container
- **CORS Configuration**: Enabling cross-origin requests to allow frontend applications to consume the API
- **Swagger/OpenAPI**: Automatic API documentation generation for testing and exploration
- **SQLite**: Working with a lightweight, file-based relational database perfect for development and small applications

## Development

- **Hot reload**: Supported for both API and frontend
- **CORS**: Configured for localhost origins during development
- **Static files**: Frontend served from `wwwroot/` directory
- **API documentation**: Available via Swagger UI

