using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class Customer
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = "";
    
    [MaxLength(200)]
    public string Email { get; set; } = "";
    
    [Required]
    [MaxLength(50)]
    public string Phone { get; set; } = "";

    public Customer(string name, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone is required", nameof(phone));

        Name = name;
        Email = email;
        Phone = phone;
    }

    // For Dapper
    public Customer()
    {
    }
}

