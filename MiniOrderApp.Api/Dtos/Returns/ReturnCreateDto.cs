using System.ComponentModel.DataAnnotations;

namespace MiniOrderApp.Api.Dtos.Returns;

public record ReturnCreateDto(
    [Range(1, int.MaxValue, ErrorMessage = "Order ID must be greater than 0")]
    int OrderId,

    [MinLength(2, ErrorMessage = "Reason must be at least 2 characters")]
    [MaxLength(500)]
    string Reason
);
