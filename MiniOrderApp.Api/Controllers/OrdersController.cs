using Microsoft.AspNetCore.Mvc;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;

    public OrdersController(IOrderRepository orderRepository, ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }

    // GET: api/orders
    [HttpGet]
    public ActionResult<IEnumerable<Order>> GetAll()
    {
        var orders = _orderRepository.GetOrders();
        return Ok(orders);
    }

    // GET: api/orders/5
    [HttpGet("{id}")]
    public ActionResult<Order> GetById(int id)
    {
        var order = _orderRepository.GetById(id);
        if (order == null)
        {
            return NotFound($"Order with ID {id} not found.");
        }
        return Ok(order);
    }

    // GET: api/orders/5/items
    [HttpGet("{id}/items")]
    public ActionResult<IEnumerable<OrderItem>> GetOrderItems(int id)
    {
        var order = _orderRepository.GetById(id);
        if (order == null)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        var items = _orderRepository.GetItemsForOrder(id);
        return Ok(items);
    }

    // POST: api/orders
    [HttpPost]
    public ActionResult<Order> Create([FromBody] OrderCreateDto dto)
    {
        var customer = _customerRepository.GetById(dto.CustomerId);
        if (customer == null)
        {
            return BadRequest($"Customer with ID {dto.CustomerId} not found.");
        }

        var order = new Order(dto.CustomerId, DateTime.Now, 0);

        foreach (var itemDto in dto.Items)
        {
            var item = new OrderItem(itemDto.ProductName, itemDto.Quantity, itemDto.UnitPrice);
            order.AddItem(item);
        }

        _orderRepository.Add(order);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    // PUT: api/orders/5/status
    [HttpPut("{id}/status")]
    public ActionResult UpdateStatus(int id, [FromBody] OrderStatusUpdateDto dto)
    {
        var order = _orderRepository.GetById(id);
        if (order == null)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        order.SetStatus(dto.Status);
        _orderRepository.Update(order);
        return NoContent();
    }

    // DELETE: api/orders/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var order = _orderRepository.GetById(id);
        if (order == null)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        _orderRepository.Delete(id);
        return NoContent();
    }
}

// DTOs
public record OrderCreateDto(int CustomerId, List<OrderItemDto> Items);
public record OrderItemDto(string ProductName, int Quantity, decimal UnitPrice);
public record OrderStatusUpdateDto(OrderStatus Status);
