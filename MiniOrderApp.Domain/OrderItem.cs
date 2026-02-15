using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class OrderItem
{
    public int Id { get; private set; }

    [Required]
    public int OrderId { get; private set; }

    private string _productName = string.Empty;

    public string ProductName
    {
        get => _productName;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product name is required", nameof(ProductName));
            if (value.Length > 200)
                throw new ArgumentException("Product name cannot exceed 200 characters", nameof(ProductName));
            _productName = value;
        }
    }

    private int _quantity;

    public int Quantity
    {
        get => _quantity;
        private set
        {
            if (value <= 0)
                throw new ArgumentException("Quantity must be positive", nameof(Quantity));
            _quantity = value;
        }
    }

    private decimal _unitPrice;
    public decimal UnitPrice
    {
        get => _unitPrice;
        private set
        {
            if (value < 0)
                throw new ArgumentException("Unit price cannot be negative", nameof(UnitPrice));
            _unitPrice = value;
        }
    }

    public decimal LineTotal => Quantity * UnitPrice;

    public OrderItem(string productName, int quantity, decimal unitPrice)
    {
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void UpdateDetails(string productName, int quantity, decimal unitPrice)
    {
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    // Parameterless constructor for EF
    public OrderItem()
    {
    }
}
