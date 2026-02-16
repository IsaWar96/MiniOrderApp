using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class Customer
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string Phone { get; private set; } = string.Empty;

    public Customer(string name, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));
        if (email.Length > 200)
            throw new ArgumentException("Email cannot exceed 200 characters", nameof(email));
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone is required", nameof(phone));

        Name = name;
        Email = email;
        Phone = phone;
    }
    public void UpdateDetails(string name, string email, string phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
    }

    // Parameterless constructor for EF Core
    private Customer()
    {
    }
}

