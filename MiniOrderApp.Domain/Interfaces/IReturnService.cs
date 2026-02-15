using MiniOrderApp.Domain;

namespace MiniOrderApp.Domain.Interfaces;

public interface IReturnService
{
    Task<IEnumerable<Return>> GetAllReturnsAsync();
    Task<Return> CreateReturnAsync(int orderId, string reason);
    Task<Return> GetReturnByOrderIdAsync(int orderId);
}
