namespace MiniOrderApp.Domain;

public class Return
{
    public int Id { get; set; }
    public int OrderId { get; private set; }
    public DateTime ReturnDate { get; private set; }
    public string Reason { get; private set; } = "";
    public decimal RefundedAmount { get; private set; }

    public Return(int orderId, DateTime returnDate, string reason, decimal refundedAmount)
    {
        if (orderId <= 0)
            throw new ArgumentException("OrderId must be positive", nameof(orderId));
        if (refundedAmount < 0)
            throw new ArgumentException("Refunded amount cannot be negative", nameof(refundedAmount));

        OrderId = orderId;
        ReturnDate = returnDate;
        Reason = reason ?? "";
        RefundedAmount = refundedAmount;
    }
    // For Dapper
    public Return()
    {
    }
}
