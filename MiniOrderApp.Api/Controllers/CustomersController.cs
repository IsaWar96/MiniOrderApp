using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomersController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    // GET: api/customers
    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetAll()
    {
        var customers = _customerRepository.GetCustomers();
        return Ok(customers);
    }

    // GET: api/customers/5
    [HttpGet("{id}")]
    public ActionResult<Customer> GetById(int id)
    {
        var customer = _customerRepository.GetById(id);
        if (customer == null)
        {
            return NotFound($"Customer with ID {id} not found.");
        }
        return Ok(customer);
    }

    // POST: api/customers
    [HttpPost]
    public ActionResult<Customer> Create([FromBody] CustomerCreateDto dto)
    {
        var customer = new Customer(dto.Name, dto.Email, dto.Phone);
        _customerRepository.Add(customer);
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    // PUT: api/customers/5
    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] CustomerUpdateDto dto)
    {
        var customer = _customerRepository.GetById(id);
        if (customer == null)
        {
            return NotFound($"Customer with ID {id} not found.");
        }

        customer.Name = dto.Name;
        customer.Email = dto.Email;
        customer.Phone = dto.Phone;

        _customerRepository.Update(customer);
        return NoContent();
    }

    // DELETE: api/customers/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var customer = _customerRepository.GetById(id);
        if (customer == null)
        {
            return NotFound($"Customer with ID {id} not found.");
        }

        _customerRepository.Delete(id);
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
