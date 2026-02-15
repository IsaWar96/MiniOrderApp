namespace MiniOrderApp.Domain.Interfaces;

public interface IReturnRepository
{
    Task<IEnumerable<Return>> GetAllAsync();
    Task AddReturnAsync(Return returnInfo);
    Task<Return?> GetByOrderIdAsync(int id);
}

