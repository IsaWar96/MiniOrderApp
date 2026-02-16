using Microsoft.AspNetCore.Mvc;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Api.Dtos.Orders;

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
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAll()
    {
        var orders = await _orderService.GetAllOrdersAsync();

        var result = orders.Select(o =>
            new OrderResponseDto(
                o.Id,
                o.CustomerId,
                o.OrderDate,
                o.Status,
                o.TotalAmount
            )
        );

        return Ok(result);
    }

    // GET: api/orders/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound();

        return Ok(new OrderResponseDto(
            order.Id,
            order.CustomerId,
            order.OrderDate,
            order.Status,
            order.TotalAmount
        ));
    }

    // GET: api/orders/5/items
    [HttpGet("{id}/items")]
    public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems(int id)
    {
        var items = await _orderService.GetOrderItemsAsync(id);

        if (items == null)
            return NotFound();

        var result = items.Select(i =>
            new OrderItemDto(
                i.ProductName,
                i.Quantity,
                i.UnitPrice
            )
        );

        return Ok(result);
    }

    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> Create(OrderCreateDto dto)
    {
        var items = dto.Items.Select(i =>
            new OrderItem(i.ProductName, i.Quantity, i.UnitPrice)
        ).ToList();

        var createdOrder = await _orderService.CreateOrderAsync(dto.CustomerId, items);

        var response = new OrderResponseDto(
            createdOrder.Id,
            createdOrder.CustomerId,
            createdOrder.OrderDate,
            createdOrder.Status,
            createdOrder.TotalAmount
        );

        return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, response);
    }

    // PUT: api/orders/5/status
    [HttpPut("{id}/status")]
    public async Task<ActionResult> UpdateStatus(int id, OrderStatusUpdateDto dto)
    {
        var order = await _orderService.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound();

        await _orderService.UpdateOrderAsync(id, order.CustomerId, order.OrderDate, dto.Status);

        return NoContent();
    }

    // DELETE: api/orders/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound();

        await _orderService.DeleteOrderAsync(id);

        return NoContent();
    }
}
