using MiniOrderApp.Domain;

namespace MiniOrderApp.Infrastructure.Database;

public static class DbSeeder
{
    public static void SeedData(ApplicationDbContext context)
    {
        if (context.Customers.Any())
        {
            return;
        }

        var customers = new[]
        {
            new Customer { Name = "John Doe", Email = "john.doe@example.com", Phone = "555-0101" },
            new Customer { Name = "Jane Smith", Email = "jane.smith@example.com", Phone = "555-0102" },
            new Customer { Name = "Bob Johnson", Email = "bob.johnson@example.com", Phone = "555-0103" },
            new Customer { Name = "Alice Williams", Email = "alice.williams@example.com", Phone = "555-0104" },
            new Customer { Name = "Charlie Brown", Email = "charlie.brown@example.com", Phone = "555-0105" }
        };

        context.Customers.AddRange(customers);
        context.SaveChanges();

        var orders = new[]
        {
            new Order 
            { 
                CustomerId = 1, 
                OrderDate = DateTime.Now.AddDays(-10), 
                Status = OrderStatus.Paid,
                TotalAmount = 0
            },
            new Order 
            { 
                CustomerId = 2, 
                OrderDate = DateTime.Now.AddDays(-5), 
                Status = OrderStatus.Paid,
                TotalAmount = 0
            },
            new Order 
            { 
                CustomerId = 3, 
                OrderDate = DateTime.Now.AddDays(-2), 
                Status = OrderStatus.Created,
                TotalAmount = 0
            }
        };

        context.Orders.AddRange(orders);
        context.SaveChanges();

        var orderItems = new[]
        {
            new OrderItem { OrderId = 1, ProductName = "Laptop", Quantity = 1, UnitPrice = 999.99m },
            new OrderItem { OrderId = 1, ProductName = "Mouse", Quantity = 2, UnitPrice = 25.50m },
            new OrderItem { OrderId = 2, ProductName = "Keyboard", Quantity = 1, UnitPrice = 79.99m },
            new OrderItem { OrderId = 2, ProductName = "Monitor", Quantity = 1, UnitPrice = 299.99m },
            new OrderItem { OrderId = 3, ProductName = "USB Cable", Quantity = 3, UnitPrice = 12.99m },
            new OrderItem { OrderId = 3, ProductName = "Headphones", Quantity = 1, UnitPrice = 149.99m }
        };

        context.OrderItems.AddRange(orderItems);
        context.SaveChanges();

        orders[0].TotalAmount = 1050.99m;
        orders[1].TotalAmount = 379.98m;
        orders[2].TotalAmount = 188.96m;
        context.SaveChanges();
    }
}
