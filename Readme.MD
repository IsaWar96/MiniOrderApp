# MiniOrderApp

MiniOrderApp is a full-stack order management system built with ASP.NET Core Web API, Entity Framework Core, SQLite, and modern web technologies (HTML, CSS, JavaScript).

The application follows a clean architecture pattern with separate projects for Domain, Infrastructure, API, Presentation (console UI), and Tests.

## Features

- **RESTful Web API** with Swagger/OpenAPI documentation
- **Interactive Web Frontend** with modern UI design
- **Entity Framework Core** for database access with SQLite
- **CRUD operations** for customers, orders, and returns
- **Order management** with multiple items per order
- **Return processing** with reason tracking and refund amounts
- **CORS enabled** for API access
- **Integration tests** for repositories

## Architecture

### Projects

- **MiniOrderApp.Domain**: Core business entities and interfaces
- **MiniOrderApp.Infrastructure**: EF Core DbContext and repository implementations
- **MiniOrderApp.Api**: ASP.NET Core Web API with controllers and frontend
- **MiniOrderApp.Presentation**: Console-based UI (legacy)
- **MiniOrderApp.Tests**: Integration tests

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

6. **Run tests (optional)**
   ```bash
   dotnet test
   ```

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
- `GET /api/returns/order/{orderId}` - Get return by order ID
- `POST /api/returns` - Create new return

## Configuration

The application uses SQLite with a connection string configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "AppDb": "Data Source=orderdata.db"
}
```

The database is automatically created on first run.

## Development

- **Hot reload**: Supported for both API and frontend
- **CORS**: Enabled for all origins in development
- **Static files**: Frontend served from `wwwroot/` directory
- **API documentation**: Available via Swagger UI

## License

Educational project for learning purposes.
