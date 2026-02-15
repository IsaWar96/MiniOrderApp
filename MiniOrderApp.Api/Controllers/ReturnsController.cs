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
    public ActionResult<IEnumerable<Return>> GetAll()
    {
        var returns = _returnService.GetAllReturns();
        return Ok(returns);
    }

    // GET: api/returns/order/5
    [HttpGet("order/{orderId}")]
    public ActionResult<Return> GetByOrderId(int orderId)
    {
        var returnInfo = _returnService.GetReturnByOrderId(orderId);
        return Ok(returnInfo);
    }

    // POST: api/returns
    [HttpPost]
    public ActionResult<Return> Create(ReturnCreateDto dto)
    {
        var returnInfo = new Return(dto.OrderId, DateTime.Now, dto.Reason, dto.RefundedAmount);
        var createdReturn = _returnService.CreateReturn(returnInfo);
        return CreatedAtAction(nameof(GetByOrderId), new { orderId = createdReturn.OrderId }, createdReturn);
    }
}

// DTOs
public record ReturnCreateDto(
    [Range(1, int.MaxValue, ErrorMessage = "Order ID must be greater than 0")]
    int OrderId,

    [MinLength(2, ErrorMessage = "Reason must be at least 2 characters")]
    [MaxLength(500)]
    string Reason,

    [Range(0, double.MaxValue, ErrorMessage = "Refunded amount cannot be negative")]
    decimal RefundedAmount
);
