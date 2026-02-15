using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class Customer
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = "";

    [EmailAddress(ErrorMessage = "Invalid email format")]
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
        if (!string.IsNullOrEmpty(email) && !IsValidEmail(email))
            throw new ArgumentException("Invalid email format", nameof(email));

        Name = name;
        Email = email;
        Phone = phone;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    // For Dapper
    public Customer()
    {
    }
}

