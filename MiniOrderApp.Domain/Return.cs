using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class Return
{
    public int Id { get; private set; }

    private int _orderId;

    public int OrderId
    {
        get => _orderId;
        private set
        {
            if (value <= 0)
                throw new ArgumentException("Order ID must be greater than 0", nameof(OrderId));
            _orderId = value;
        }
    }

    public DateTime ReturnDate { get; private set; }

    private string _reason = string.Empty;

    public string Reason
    {
        get => _reason;
        private set
        {
            if (!string.IsNullOrWhiteSpace(value) && value.Length < 2)
                throw new ArgumentException("Reason must be at least 2 characters", nameof(Reason));
            if (value != null && value.Length > 500)
                throw new ArgumentException("Reason cannot exceed 500 characters", nameof(Reason));
            _reason = value ?? string.Empty;
        }
    }

    private decimal _refundedAmount;
    public decimal RefundedAmount
    {
        get => _refundedAmount;
        private set
        {
            if (value < 0)
                throw new ArgumentException("Refunded amount cannot be negative", nameof(RefundedAmount));
            _refundedAmount = value;
        }
    }

    public Return(int orderId, DateTime returnDate, string reason, decimal refundedAmount)
    {
        OrderId = orderId;
        ReturnDate = returnDate;
        Reason = reason;
        RefundedAmount = refundedAmount;
    }

    public void UpdateDetails(DateTime returnDate, string reason, decimal refundedAmount)
    {
        ReturnDate = returnDate;
        Reason = reason;
        RefundedAmount = refundedAmount;
    }

    // Parameterless constructor for EF
    public Return()
    {
    }
}
