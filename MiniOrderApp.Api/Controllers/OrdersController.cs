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
        try
        {
            var orders = _orderRepository.GetOrders();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET: api/orders/5
    [HttpGet("{id}")]
    public ActionResult<Order> GetById(int id)
    {
        try
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }
            return Ok(order);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET: api/orders/5/items
    [HttpGet("{id}/items")]
    public ActionResult<IEnumerable<OrderItem>> GetOrderItems(int id)
    {
        try
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            var items = _orderRepository.GetItemsForOrder(id);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST: api/orders
    [HttpPost]
    public ActionResult<Order> Create([FromBody] OrderCreateDto dto)
    {
        try
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
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // PUT: api/orders/5/status
    [HttpPut("{id}/status")]
    public ActionResult UpdateStatus(int id, [FromBody] OrderStatusUpdateDto dto)
    {
        try
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
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE: api/orders/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            _orderRepository.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

// DTOs
public record OrderCreateDto(int CustomerId, List<OrderItemDto> Items);
public record OrderItemDto(string ProductName, int Quantity, decimal UnitPrice);
public record OrderStatusUpdateDto(OrderStatus Status);
