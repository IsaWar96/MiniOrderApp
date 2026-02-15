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
        try
        {
            var customers = _customerRepository.GetCustomers();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET: api/customers/5
    [HttpGet("{id}")]
    public ActionResult<Customer> GetById(int id)
    {
        try
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }
            return Ok(customer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST: api/customers
    [HttpPost]
    public ActionResult<Customer> Create([FromBody] CustomerCreateDto dto)
    {
        try
        {
            var customer = new Customer(dto.Name, dto.Email, dto.Phone);
            _customerRepository.Add(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
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

    // PUT: api/customers/5
    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] CustomerUpdateDto dto)
    {
        try
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
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE: api/customers/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            _customerRepository.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
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
