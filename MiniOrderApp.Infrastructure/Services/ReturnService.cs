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

        var existingReturn = await _returnRepository.GetByOrderIdAsync(orderId);

        if (existingReturn != null)
        {
            throw new InvalidOperationException($"A return already exists for order ID {orderId}.");
        }

        // Domain entity will validate reason and other invariants
        var returnInfo = new Return(orderId, DateTime.UtcNow, reason, order.TotalAmount);

        // Mark order as returned using domain method
        order.MarkAsReturned();

        // Add both changes to the context
        await _returnRepository.AddReturnAsync(returnInfo);
        await _orderRepository.UpdateAsync(order);

        // Save changes once - ensures transactional consistency
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
