using MiniOrderApp.Domain;

namespace MiniOrderApp.Domain.Interfaces;

public interface IReturnService
{
    IEnumerable<Return> GetAllReturns();
    Return CreateReturn(Return returnInfo);
    Return GetReturnByOrderId(int orderId);
}
