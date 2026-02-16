using Microsoft.AspNetCore.Mvc;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Api.Dtos.Returns;

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
    public async Task<ActionResult<IEnumerable<ReturnResponseDto>>> GetAll()
    {
        var returns = await _returnService.GetAllReturnsAsync();

        var result = returns.Select(r =>
            new ReturnResponseDto(
                r.Id,
                r.OrderId,
                r.Reason,
                r.ReturnDate,
                r.RefundedAmount
            )
        );

        return Ok(result);
    }

    // GET: api/returns/order/5
    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<ReturnResponseDto>> GetByOrderId(int orderId)
    {
        var returnInfo = await _returnService.GetReturnByOrderIdAsync(orderId);

        if (returnInfo == null)
            return NotFound();

        return Ok(new ReturnResponseDto(
            returnInfo.Id,
            returnInfo.OrderId,
            returnInfo.Reason,
            returnInfo.ReturnDate,
            returnInfo.RefundedAmount
        ));
    }

    // POST: api/returns
    [HttpPost]
    public async Task<ActionResult<ReturnResponseDto>> Create(ReturnCreateDto dto)
    {
        var createdReturn = await _returnService.CreateReturnAsync(dto.OrderId, dto.Reason);

        var response = new ReturnResponseDto(
            createdReturn.Id,
            createdReturn.OrderId,
            createdReturn.Reason,
            createdReturn.ReturnDate,
            createdReturn.RefundedAmount
        );

        return CreatedAtAction(
            nameof(GetByOrderId),
            new { orderId = createdReturn.OrderId },
            response
        );
    }
}
