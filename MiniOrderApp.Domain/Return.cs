using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class Return
{
    public int Id { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Order ID must be greater than 0")]
    public int OrderId { get; set; }

    [Required]
    public DateTime ReturnDate { get; set; }

    [MinLength(2, ErrorMessage = "Reason must be at least 2 characters")]
    [MaxLength(500)]
    public string Reason { get; set; } = "";

    [Required]
    public decimal RefundedAmount { get; set; }

    public Return(int orderId, DateTime returnDate, string reason, decimal refundedAmount)
    {
        if (orderId <= 0)
            throw new ArgumentException("Order ID must be greater than 0", nameof(orderId));
        if (refundedAmount < 0)
            throw new ArgumentException("Refunded amount cannot be negative", nameof(refundedAmount));
        if (!string.IsNullOrEmpty(reason) && reason.Length < 2)
            throw new ArgumentException("Reason must be at least 2 characters", nameof(reason));

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
