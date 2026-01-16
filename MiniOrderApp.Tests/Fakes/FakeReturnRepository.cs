using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Tests.Fakes;

public class FakeReturnRepository : IReturnRepository
{
    private readonly List<Return> _returns = new();
    private int _nextId = 1;

    public void AddReturn(Return returnInfo)
    {
        if (returnInfo == null) throw new ArgumentNullException(nameof(returnInfo));
        
        returnInfo.Id = _nextId++;
        _returns.Add(returnInfo);
    }

    public Return? GetByOrderId(int orderId)
    {
        return _returns.FirstOrDefault(r => r.OrderId == orderId);
    }
}
