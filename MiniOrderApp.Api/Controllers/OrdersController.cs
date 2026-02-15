using Microsoft.AspNetCore.Mvc;
using MiniOrderApp.Domain;
using MiniOrderApp.Infrastructure.Interfaces;

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
    public ActionResult<IEnumerable<Order>> GetAll()
    {
        var orders = _orderService.GetAllOrders();
        return Ok(orders);
    }

    // GET: api/orders/5
    [HttpGet("{id}")]
    public ActionResult<Order> GetById(int id)
    {
        var order = _orderService.GetOrderById(id);
        return Ok(order);
    }

    // GET: api/orders/5/items
    [HttpGet("{id}/items")]
    public ActionResult<IEnumerable<OrderItem>> GetOrderItems(int id)
    {
        var items = _orderService.GetOrderItems(id);
        return Ok(items);
    }

    // POST: api/orders
    [HttpPost]
    public ActionResult<Order> Create([FromBody] OrderCreateDto dto)
    {
        var order = new Order(dto.CustomerId, DateTime.Now, 0);

        foreach (var itemDto in dto.Items)
        {
            var item = new OrderItem(itemDto.ProductName, itemDto.Quantity, itemDto.UnitPrice);
            order.AddItem(item);
        }

        var createdOrder = _orderService.CreateOrder(order);
        return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
    }

    // PUT: api/orders/5/status
    [HttpPut("{id}/status")]
    public ActionResult UpdateStatus(int id, [FromBody] OrderStatusUpdateDto dto)
    {
        var order = _orderService.GetOrderById(id);
        order.SetStatus(dto.Status);
        _orderService.UpdateOrder(id, order);
        return NoContent();
    }

    // DELETE: api/orders/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _orderService.DeleteOrder(id);
        return NoContent();
    }
}

// DTOs
public record OrderCreateDto(int CustomerId, List<OrderItemDto> Items);
public record OrderItemDto(string ProductName, int Quantity, decimal UnitPrice);
public record OrderStatusUpdateDto(OrderStatus Status);
