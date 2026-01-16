using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Tests.Fakes;

namespace MiniOrderApp.Tests;

public class CustomerRepositoryTests
{
    private ICustomerRepository CreateRepository()
    {
        return new FakeCustomerRepository();
    }

    [Fact]
    public void GetCustomers_Should_Return_Customers_From_Database()
    {
        // Arrange
        var repo = CreateRepository();

        var customer1 = new Customer(
            "Customer 1",
            "customer1@test.com",
            "123456789"
        );
        var customer2 = new Customer(
            "Customer 2",
            "customer2@test.com",
            "987654321"
        );

        repo.Add(customer1);
        repo.Add(customer2);

        // Act
        var customers = repo.GetCustomers().ToList();

        // Assert
        Assert.Equal(2, customers.Count);

        bool foundCustomer1 = false;
        bool foundCustomer2 = false;

        foreach (var customer in customers)
        {
            if (customer.Name == "Customer 1")
            {
                foundCustomer1 = true;
            }
            else if (customer.Name == "Customer 2")
            {
                foundCustomer2 = true;
            }

            if (foundCustomer1 && foundCustomer2)
            {
                break;
            }
        }

        Assert.True(foundCustomer1);
        Assert.True(foundCustomer2);
    }

    [Fact]
    public void GetById_Should_Return_Single_Customer()
    {
        // Arrange
        var repo = CreateRepository();

        var customer = new Customer(
            "Single Customer",
            "single@test.com",
            "123456789"
        );

        repo.Add(customer);
        int id = customer.Id;

        // Act
        var retrieved = repo.GetById(id);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(id, retrieved!.Id);
        Assert.Equal("Single Customer", retrieved.Name);
    }

    [Fact]
    public void Add_Should_Insert_Customer_In_Database()
    {
        // Arrange
        var repo = CreateRepository();

        var customer = new Customer(
            "New Customer",
            "new@test.com",
            "123456789"
        );

        // Act
        repo.Add(customer);

        // Assert
        var customers = repo.GetCustomers().ToList();
        Assert.Single(customers);

        var saved = customers.Single();
        Assert.Equal("New Customer", saved.Name);
        Assert.Equal("new@test.com", saved.Email);
        Assert.Equal("123456789", saved.Phone);
    }

    [Fact]
    public void Delete_Should_Remove_Customer_From_Database()
    {
        // Arrange
        var repo = CreateRepository();

        var customer = new Customer(
            "Customer To Delete",
            "delete@test.com",
            "123456789"
        );

        repo.Add(customer);
        int id = customer.Id;

        // Act
        repo.Delete(id);

        // Assert
        var customers = repo.GetCustomers().ToList();
        bool customerExists = customers.Any(c => c.Id == id);

        Assert.False(customerExists, "Customer should have been deleted but still exists.");
    }
}


