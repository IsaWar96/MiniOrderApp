namespace MiniOrderApp.Api.Dtos.Returns;

public record ReturnResponseDto(
    int Id,
    int OrderId,
    string Reason,
    DateTime ReturnDate,
    decimal RefundedAmount
);
