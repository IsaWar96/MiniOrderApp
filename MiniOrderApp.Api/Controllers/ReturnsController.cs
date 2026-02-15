using Microsoft.AspNetCore.Mvc;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReturnsController : ControllerBase
{
    private readonly IReturnRepository _returnRepository;
    private readonly IOrderRepository _orderRepository;

    public ReturnsController(IReturnRepository returnRepository, IOrderRepository orderRepository)
    {
        _returnRepository = returnRepository;
        _orderRepository = orderRepository;
    }

    // GET: api/returns/order/5
    [HttpGet("order/{orderId}")]
    public ActionResult<Return> GetByOrderId(int orderId)
    {
        try
        {
            var returnInfo = _returnRepository.GetByOrderId(orderId);
            if (returnInfo == null)
            {
                return NotFound($"No return found for order ID {orderId}.");
            }
            return Ok(returnInfo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST: api/returns
    [HttpPost]
    public ActionResult<Return> Create([FromBody] ReturnCreateDto dto)
    {
        try
        {
            var order = _orderRepository.GetById(dto.OrderId);
            if (order == null)
            {
                return BadRequest($"Order with ID {dto.OrderId} not found.");
            }

            var returnInfo = new Return(dto.OrderId, DateTime.Now, dto.Reason, dto.RefundedAmount);
            _returnRepository.AddReturn(returnInfo);

            // Mark the order as returned
            _orderRepository.MarkAsReturned(dto.OrderId);

            return CreatedAtAction(nameof(GetByOrderId), new { orderId = returnInfo.OrderId }, returnInfo);
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
}

// DTOs
public record ReturnCreateDto(int OrderId, string Reason, decimal RefundedAmount);
