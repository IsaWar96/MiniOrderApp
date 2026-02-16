using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class OrderItem
{
    public int Id { get; private set; }

    [Required]
    public int OrderId { get; private set; }

    public string ProductName { get; private set; } = string.Empty;

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal LineTotal => Quantity * UnitPrice;

    public OrderItem(string productName, int quantity, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required", nameof(productName));
        if (productName.Length > 200)
            throw new ArgumentException("Product name cannot exceed 200 characters", nameof(productName));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));

        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    // Parameterless constructor for EF
    private OrderItem()
    {
    }
}
