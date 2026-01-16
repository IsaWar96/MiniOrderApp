using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Tests.Fakes;

namespace MiniOrderApp.Tests;

public class OrderRepositoryTests
{
    private IOrderRepository CreateRepository()
    {
        return new FakeOrderRepository();
    }

    [Fact]
    public void GetOrders_Should_Return_Order_From_Database()
    {
        // Arrange
        var repo = CreateRepository();

        var order = new Order(
            1,
            DateTime.Today,
            200m
        );

        repo.Add(order);
        int orderId = order.Id;

        // Act
        var orders = repo.GetOrders().ToList();
        var retrieved = orders.Single();

        // Assert
        Assert.Equal(orderId, retrieved.Id);
        Assert.Equal(1, retrieved.CustomerId);
        Assert.Equal(OrderStatus.Created, retrieved.Status);
    }

    [Fact]
    public void Add_Should_Insert_Order_In_Database()
    {
        // Arrange
        var repo = CreateRepository();

        // Create an order in the domain layer
        var order = new Order(
            1,
            DateTime.Today,
            200m
        );

        // Act
        repo.Add(order);

        // Assert
        var orders = repo.GetOrders().ToList();

        Assert.Single(orders);
        var saved = orders.Single();

        Assert.Equal(1, saved.CustomerId);
        Assert.Equal(OrderStatus.Created, saved.Status);
        Assert.Equal(200m, saved.TotalAmount);
    }

    [Fact]
    public void Delete_Should_Remove_Order_From_Database()
    {
        // Arrange
        var repo = CreateRepository();

        var order = new Order(
            1,
            DateTime.Today,
            200m
        );

        repo.Add(order);
        int orderId = order.Id;

        // Act
        repo.Delete(orderId);

        // Assert
        var orders = repo.GetOrders().ToList();
        Assert.Empty(orders);
    }

    [Fact]
    public void MarkAsReturned_Should_Update_Status_To_Returned()
    {
        // Arrange
        var repo = CreateRepository();

        var order = new Order(
            1,
            DateTime.Today,
            200m
        );

        repo.Add(order);
        int orderId = order.Id;

        // Act
        repo.MarkAsReturned(orderId);

        // Assert
        var updated = repo.GetById(orderId);

        Assert.NotNull(updated);
        Assert.Equal(OrderStatus.Returned, updated!.Status);
    }
}

