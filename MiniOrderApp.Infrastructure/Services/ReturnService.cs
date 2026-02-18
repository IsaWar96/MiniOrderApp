using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;

namespace MiniOrderApp.Infrastructure.Services;

public class ReturnService : IReturnService
{
    private readonly IReturnRepository _returnRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ApplicationDbContext _context;

    public ReturnService(IReturnRepository returnRepository, IOrderRepository orderRepository, ApplicationDbContext context)
    {
        _returnRepository = returnRepository;
        _orderRepository = orderRepository;
        _context = context;
    }

    public async Task<IEnumerable<Return>> GetAllReturnsAsync()
    {
        var returns = await _returnRepository.GetAllAsync();
        if (returns == null)
        {
            throw new ArgumentException("Returns cannot be null", nameof(returns));
        }

        return returns;
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

        var existingReturn = await _returnRepository.GetByOrderIdAsync(orderId);

        if (existingReturn != null)
        {
            throw new InvalidOperationException($"A return already exists for order ID {orderId}.");
        }

        var returnInfo = new Return(orderId, DateTime.UtcNow, reason, order.TotalAmount);

        order.MarkAsReturned();

        await _returnRepository.AddReturnAsync(returnInfo);
        await _orderRepository.UpdateAsync(order);

        await _context.SaveChangesAsync();

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
