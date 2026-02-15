using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class OrderItem
{
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = "";

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    public decimal LineTotal => Quantity * UnitPrice;

    public OrderItem(string productName, int quantity, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required", nameof(productName));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));

        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
    // For Dapper
    public OrderItem()
    {
    }
}
