using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Interfaces;

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

    public IEnumerable<Return> GetAllReturns()
    {
        return _returnRepository.GetAll();
    }

    public Return CreateReturn(Return returnInfo)
    {
        if (returnInfo == null)
        {
            throw new ArgumentNullException(nameof(returnInfo), "Return information cannot be null.");
        }

        if (returnInfo.OrderId <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(returnInfo.OrderId));
        }

        var order = _orderRepository.GetById(returnInfo.OrderId);
        
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {returnInfo.OrderId} not found.");
        }

        if (order.Status == OrderStatus.Returned)
        {
            throw new InvalidOperationException($"Order with ID {returnInfo.OrderId} has already been returned.");
        }

        var existingReturn = _returnRepository.GetByOrderId(returnInfo.OrderId);
        
        if (existingReturn != null)
        {
            throw new InvalidOperationException($"A return already exists for order ID {returnInfo.OrderId}.");
        }

        if (string.IsNullOrWhiteSpace(returnInfo.Reason))
        {
            throw new ArgumentException("Return reason is required.", nameof(returnInfo.Reason));
        }

        returnInfo.ReturnDate = DateTime.Now;
        returnInfo.RefundAmount = order.TotalAmount;

        _returnRepository.AddReturn(returnInfo);
        _orderRepository.MarkAsReturned(returnInfo.OrderId);

        return returnInfo;
    }

    public Return GetReturnByOrderId(int orderId)
    {
        if (orderId <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(orderId));
        }

        var order = _orderRepository.GetById(orderId);
        
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }

        var returnInfo = _returnRepository.GetByOrderId(orderId);
        
        if (returnInfo == null)
        {
            throw new KeyNotFoundException($"No return found for order ID {orderId}.");
        }

        return returnInfo;
    }
}
