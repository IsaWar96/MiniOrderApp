using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class Order
{
    public int Id { get; private set; }

    private int _customerId;
    [Required]
    public int CustomerId
    {
        get => _customerId;
        private set
        {
            if (value <= 0)
                throw new ArgumentException("CustomerId must be positive", nameof(CustomerId));
            _customerId = value;
        }
    }

    [Required]
    public DateTime OrderDate { get; private set; }

    [Required]
    public OrderStatus Status { get; private set; }

    private decimal _totalAmount;
    [Required]
    public decimal TotalAmount
    {
        get => _totalAmount;
        private set
        {
            if (value < 0)
                throw new ArgumentException("TotalAmount cannot be negative", nameof(TotalAmount));
            _totalAmount = value;
        }
    }

    // Holds items in the order
    public List<OrderItem> Items { get; } = new();

    public Order(int customerId, DateTime orderDate, decimal totalAmount)
    {
        CustomerId = customerId;
        OrderDate = orderDate;
        Status = OrderStatus.Created;
        TotalAmount = totalAmount;
    }

    public Order()
    {
    }

    public void AddItem(OrderItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        Items.Add(item);
        RecalculateTotal();
    }

    public void SetStatus(OrderStatus status)
    {
        Status = status;
    }

    public void MarkAsReturned()
    {
        if (Status == OrderStatus.Returned)
        {
            throw new InvalidOperationException("Order is already marked as returned.");
        }
        Status = OrderStatus.Returned;
    }

    public void UpdateDetails(int customerId, DateTime orderDate, OrderStatus status)
    {
        CustomerId = customerId;
        OrderDate = orderDate;
        Status = status;
    }

    public void RecalculateTotal()
    {
        TotalAmount = 0;
        foreach (var i in Items)
        {
            TotalAmount += i.LineTotal;
        }
    }
}

