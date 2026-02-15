using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReturnsController : ControllerBase
{
    private readonly IReturnService _returnService;

    public ReturnsController(IReturnService returnService)
    {
        _returnService = returnService;
    }

    // GET: api/returns
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Return>>> GetAll()
    {
        var returns = await _returnService.GetAllReturnsAsync();
        return Ok(returns);
    }

    // GET: api/returns/order/5
    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<Return>> GetByOrderId(int orderId)
    {
        var returnInfo = await _returnService.GetReturnByOrderIdAsync(orderId);
        return Ok(returnInfo);
    }

    // POST: api/returns
    [HttpPost]
    public async Task<ActionResult<Return>> Create(ReturnCreateDto dto)
    {
        var createdReturn = await _returnService.CreateReturnAsync(dto.OrderId, dto.Reason);
        return CreatedAtAction(nameof(GetByOrderId), new { orderId = createdReturn.OrderId }, createdReturn);
    }
}

// DTOs
public record ReturnCreateDto(
    [Range(1, int.MaxValue, ErrorMessage = "Order ID must be greater than 0")]
    int OrderId,

    [MinLength(2, ErrorMessage = "Reason must be at least 2 characters")]
    [MaxLength(500)]
    string Reason
);
