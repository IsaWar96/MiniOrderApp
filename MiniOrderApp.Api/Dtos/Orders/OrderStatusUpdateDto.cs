using MiniOrderApp.Domain;

namespace MiniOrderApp.Api.Dtos.Orders;

public record OrderStatusUpdateDto(
    OrderStatus Status
);
