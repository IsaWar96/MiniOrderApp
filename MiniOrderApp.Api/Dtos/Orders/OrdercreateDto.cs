namespace MiniOrderApp.Api.Dtos.Orders;

public record OrderCreateDto(
    int CustomerId,
    List<OrderItemDto> Items
);
