using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

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
    public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    // GET: api/customers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetById(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        return Ok(customer);
    }

    // POST: api/customers
    [HttpPost]
    public async Task<ActionResult<Customer>> Create(CustomerCreateDto dto)
    {
        var createdCustomer = await _customerService.CreateCustomerAsync(dto.Name, dto.Email, dto.Phone);
        return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
    }

    // PUT: api/customers/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CustomerUpdateDto dto)
    {
        await _customerService.UpdateCustomerAsync(id, dto.Name, dto.Email, dto.Phone);
        return NoContent();
    }

    // DELETE: api/customers/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _customerService.DeleteCustomerAsync(id);
        return NoContent();
    }
}

// DTOs
public record CustomerCreateDto(
    [Required][MaxLength(200)] string Name,
    [EmailAddress(ErrorMessage = "Invalid email format")][MaxLength(200)] string Email,
    [Required][MaxLength(50)] string Phone
);

public record CustomerUpdateDto(
    [Required][MaxLength(200)] string Name,
    [EmailAddress(ErrorMessage = "Invalid email format")][MaxLength(200)] string Email,
    [Required][MaxLength(50)] string Phone
);
