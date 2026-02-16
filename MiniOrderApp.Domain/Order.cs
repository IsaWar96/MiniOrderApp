using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class Order
{
    public int Id { get; private set; }

    [Required]
    public int CustomerId { get; private set; }

    [Required]
    public DateTime OrderDate { get; private set; }

    [Required]
    public OrderStatus Status { get; private set; }

    [Required]
    public decimal TotalAmount { get; private set; }

    // Holds items in the order
    public List<OrderItem> Items { get; } = new();

    public Order(int customerId, DateTime orderDate, decimal totalAmount)
    {
        if (customerId <= 0)
            throw new ArgumentException("CustomerId must be positive", nameof(customerId));
        if (totalAmount < 0)
            throw new ArgumentException("TotalAmount cannot be negative", nameof(totalAmount));

        CustomerId = customerId;
        OrderDate = orderDate;
        Status = OrderStatus.Created;
        TotalAmount = totalAmount;
    }

    private Order()
    {
    }

    public void AddItem(OrderItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        Items.Add(item);
        RecalculateTotal();
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

    private void RecalculateTotal()
    {
        TotalAmount = 0;
        foreach (var i in Items)
        {
            TotalAmount += i.LineTotal;
        }
    }
}

