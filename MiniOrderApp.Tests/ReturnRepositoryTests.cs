using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Tests.Fakes;

namespace MiniOrderApp.Tests;

public class ReturnRepositoryTests
{
    private IReturnRepository CreateRepository()
    {
        return new FakeReturnRepository();
    }

    [Fact]
    public void AddReturn_Then_GetByOrderId_Should_Return_Same_Data()
    {
        // Arrange
        var repo = CreateRepository();

        int orderId = 1;

        var returnInfo = new Return(
            orderId,
            DateTime.Today,
            "Test reason",
            100m
        );

        // Act
        repo.AddReturn(returnInfo);

        var loaded = repo.GetByOrderId(orderId);

        // Assert
        Assert.NotNull(loaded);
        Assert.Equal(orderId, loaded!.OrderId);
        Assert.Equal("Test reason", loaded.Reason);
        Assert.Equal(100m, loaded.RefundedAmount);
    }

    [Fact]
    public void GetByOrderId_Should_Return_Null_When_No_Return_Exists()
    {
        // Arrange
        var repo = CreateRepository();

        int orderId = 999;

        // Act
        var loaded = repo.GetByOrderId(orderId);

        // Assert
        Assert.Null(loaded);
    }
}

