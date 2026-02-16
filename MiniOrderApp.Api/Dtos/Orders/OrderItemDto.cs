namespace MiniOrderApp.Api.Dtos.Orders;

public record OrderItemDto(
    string ProductName,
    int Quantity,
    decimal UnitPrice
);
