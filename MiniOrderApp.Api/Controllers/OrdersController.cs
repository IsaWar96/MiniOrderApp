using Microsoft.AspNetCore.Mvc;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET: api/orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    // GET: api/orders/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        return Ok(order);
    }

    // GET: api/orders/5/items
    [HttpGet("{id}/items")]
    public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems(int id)
    {
        var items = await _orderService.GetOrderItemsAsync(id);
        return Ok(items);
    }

    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult<Order>> Create(OrderCreateDto dto)
    {
        var items = new List<OrderItem>();
        foreach (var itemDto in dto.Items)
        {
            var item = new OrderItem(itemDto.ProductName, itemDto.Quantity, itemDto.UnitPrice);
            items.Add(item);
        }

        var createdOrder = await _orderService.CreateOrderAsync(dto.CustomerId, items);
        return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
    }

    // PUT: api/orders/5/status
    [HttpPut("{id}/status")]
    public async Task<ActionResult> UpdateStatus(int id, OrderStatusUpdateDto dto)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        await _orderService.UpdateOrderAsync(id, order.CustomerId, order.OrderDate, dto.Status);
        return NoContent();
    }

    // DELETE: api/orders/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _orderService.DeleteOrderAsync(id);
        return NoContent();
    }
}

// DTOs
public record OrderCreateDto(int CustomerId, List<OrderItemDto> Items);
public record OrderItemDto(string ProductName, int Quantity, decimal UnitPrice);
public record OrderStatusUpdateDto(OrderStatus Status);
