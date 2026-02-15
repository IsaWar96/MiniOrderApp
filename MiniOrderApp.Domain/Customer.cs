using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class Customer
{
    public int Id { get; private set; }

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name is required", nameof(Name));
            _name = value;
        }
    }

    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email is required", nameof(Email));
            if (value.Length > 200)
                throw new ArgumentException("Email cannot exceed 200 characters", nameof(Email));
            _email = value;
        }
    }

    private string _phone = string.Empty;
    public string Phone
    {
        get => _phone;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Phone is required", nameof(Phone));
            _phone = value;
        }
    }

    public Customer(string name, string email, string phone)
    {
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
    public Customer()
    {
    }
}

