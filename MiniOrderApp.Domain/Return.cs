using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class Return
{
    public int Id { get; private set; }

    public int OrderId { get; private set; }

    public DateTime ReturnDate { get; private set; }

    public string Reason { get; private set; } = string.Empty;

    public decimal RefundedAmount { get; private set; }

    public Return(int orderId, DateTime returnDate, string reason, decimal refundedAmount)
    {
        if (orderId <= 0)
            throw new ArgumentException("Order ID must be greater than 0", nameof(orderId));
        if (!string.IsNullOrWhiteSpace(reason) && reason.Length < 2)
            throw new ArgumentException("Reason must be at least 2 characters", nameof(reason));
        if (reason != null && reason.Length > 500)
            throw new ArgumentException("Reason cannot exceed 500 characters", nameof(reason));
        if (refundedAmount < 0)
            throw new ArgumentException("Refunded amount cannot be negative", nameof(refundedAmount));

        OrderId = orderId;
        ReturnDate = returnDate;
        Reason = reason ?? string.Empty;
        RefundedAmount = refundedAmount;
    }

    // Parameterless constructor for EF
    private Return()
    {
    }
}
