using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Api.Dtos.Customers;

public record CustomerCreateDto(
    [Required][MaxLength(200)] string Name,
    [EmailAddress(ErrorMessage = "Invalid email format")][MaxLength(200)] string Email,
    [Required][MaxLength(50)] string Phone
);
