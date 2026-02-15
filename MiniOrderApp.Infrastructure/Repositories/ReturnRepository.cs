using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;

namespace MiniOrderApp.Infrastructure.Repositories;

public class ReturnRepository : IReturnRepository
{
    private readonly ApplicationDbContext _context;

    public ReturnRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddReturn(Return returnInfo)
    {
        _context.Returns.Add(returnInfo);
        _context.SaveChanges();
    }

    public Return? GetByOrderId(int orderId)
    {
        return _context.Returns.FirstOrDefault(r => r.OrderId == orderId);
    }
}
