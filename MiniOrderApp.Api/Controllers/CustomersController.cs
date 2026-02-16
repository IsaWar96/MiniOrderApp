using Microsoft.AspNetCore.Mvc;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Api.Dtos.Customers;

namespace MiniOrderApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET: api/customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAll()
    {
        var customers = await _customerService.GetAllCustomersAsync();

        var result = customers.Select(c =>
            new CustomerResponseDto(
                c.Id,
                c.Name,
                c.Email
            )
        );

        return Ok(result);
    }

    // GET: api/customers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> GetById(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound();
        }

        return Ok(new CustomerResponseDto(
            customer.Id,
            customer.Name,
            customer.Email
            )
        );
    }

    // POST: api/customers
    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> Create(CustomerCreateDto dto)
    {
        var createdCustomer = await _customerService.CreateCustomerAsync(dto.Name, dto.Email, dto.Phone);
        return CreatedAtAction(nameof(GetById),
            new { id = createdCustomer.Id },
            new CustomerResponseDto(
            createdCustomer.Id,
            createdCustomer.Name,
            createdCustomer.Email
            )
        );
    }

    // PUT: api/customers/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CustomerUpdateDto dto)
    {
        await _customerService.UpdateCustomerAsync(
            id,
            dto.Name,
            dto.Email,
            dto.Phone
        );

        if (await _customerService.GetCustomerByIdAsync(id) == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/customers/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (await _customerService.GetCustomerByIdAsync(id) == null)
        {
            return NotFound();
        }

        await _customerService.DeleteCustomerAsync(id);
        return NoContent();
    }
}
