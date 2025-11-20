namespace MiniOrderApp.Domain;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; private set; } = "";
    public string Email { get; private set; } = "";
    public string Phone { get; private set; } = "";

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

