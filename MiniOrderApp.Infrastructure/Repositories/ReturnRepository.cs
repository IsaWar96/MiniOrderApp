using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<Return>> GetAllAsync()
    {
        return await _context.Returns.ToListAsync();
    }

    public Task AddReturnAsync(Return returnInfo)
    {
        _context.Returns.Add(returnInfo);
        return Task.CompletedTask;
    }

    public async Task<Return?> GetByOrderIdAsync(int orderId)
    {
        return await _context.Returns.FirstOrDefaultAsync(r => r.OrderId == orderId);
    }
}
