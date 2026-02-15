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
    public ActionResult<IEnumerable<Customer>> GetAll()
    {
        var customers = _customerService.GetAllCustomers();
        return Ok(customers);
    }

    // GET: api/customers/5
    [HttpGet("{id}")]
    public ActionResult<Customer> GetById(int id)
    {
        var customer = _customerService.GetCustomerById(id);
        return Ok(customer);
    }

    // POST: api/customers
    [HttpPost]
    public ActionResult<Customer> Create(CustomerCreateDto dto)
    {
        var customer = new Customer(dto.Name, dto.Email, dto.Phone);
        var createdCustomer = _customerService.CreateCustomer(customer);
        return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
    }

    // PUT: api/customers/5
    [HttpPut("{id}")]
    public ActionResult Update(int id, CustomerUpdateDto dto)
    {
        var customer = new Customer(dto.Name, dto.Email, dto.Phone);
        _customerService.UpdateCustomer(id, customer);
        return NoContent();
    }

    // DELETE: api/customers/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _customerService.DeleteCustomer(id);
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
