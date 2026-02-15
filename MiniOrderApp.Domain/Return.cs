using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Domain;

public class Return
{
    public int Id { get; set; }
    
    [Required]
    public int OrderId { get; set; }
    
    [Required]
    public DateTime ReturnDate { get; set; }
    
    [MaxLength(500)]
    public string Reason { get; set; } = "";
    
    [Required]
    public decimal RefundedAmount { get; set; }

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
