using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class Order
{
    public int Id { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    public OrderStatus Status { get; set; }

    [Required]
    public decimal TotalAmount { get; set; }

    // Holds items in the order
    public List<OrderItem> Items { get; } = new();

    public Order(int customerId, DateTime orderDate, decimal totalAmount)
    {
        if (customerId <= 0)
            throw new ArgumentException("CustomerId must be positive", nameof(customerId));

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

    public void RecalculateTotal()
    {

        foreach (var i in Items)
        {
            TotalAmount += i.LineTotal;
        }
    }
}

