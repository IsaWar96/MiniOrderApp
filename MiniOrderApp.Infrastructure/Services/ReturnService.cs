using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Infrastructure.Services;

public class ReturnService : IReturnService
{
    private readonly IReturnRepository _returnRepository;
    private readonly IOrderRepository _orderRepository;

    public ReturnService(IReturnRepository returnRepository, IOrderRepository orderRepository)
    {
        _returnRepository = returnRepository;
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<Return>> GetAllReturnsAsync()
    {
        return await _returnRepository.GetAllAsync();
    }

    public async Task<Return> CreateReturnAsync(int orderId, string reason)
    {
        if (orderId <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(orderId));
        }

        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }

        if (order.Status == OrderStatus.Returned)
        {
            throw new InvalidOperationException($"Order with ID {orderId} has already been returned.");
        }

        var existingReturn = await _returnRepository.GetByOrderIdAsync(orderId);

        if (existingReturn != null)
        {
            throw new InvalidOperationException($"A return already exists for order ID {orderId}.");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Return reason is required.", nameof(reason));
        }

        var returnInfo = new Return(orderId, DateTime.Now, reason, order.TotalAmount);

        await _returnRepository.AddReturnAsync(returnInfo);
        await _orderRepository.MarkAsReturnedAsync(orderId);

        return returnInfo;
    }

    public async Task<Return> GetReturnByOrderIdAsync(int orderId)
    {
        if (orderId <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(orderId));
        }

        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }

        var returnInfo = await _returnRepository.GetByOrderIdAsync(orderId);

        if (returnInfo == null)
        {
            throw new KeyNotFoundException($"No return found for order ID {orderId}.");
        }

        return returnInfo;
    }
}
