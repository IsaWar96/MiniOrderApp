namespace MiniOrderApp.Domain;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }

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
        decimal TotalAmount = 0;

        foreach (var i in Items)
        {
            TotalAmount += i.LineTotal;
        }
    }
}

