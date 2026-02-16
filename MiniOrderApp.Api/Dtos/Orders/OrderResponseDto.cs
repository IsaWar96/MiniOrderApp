using MiniOrderApp.Domain;

namespace MiniOrderApp.Api.Dtos.Orders;

public record OrderResponseDto(
    int Id,
    int CustomerId,
    DateTime OrderDate,
    OrderStatus Status,
    decimal TotalAmount
);
