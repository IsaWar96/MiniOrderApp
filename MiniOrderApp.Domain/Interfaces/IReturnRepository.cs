namespace MiniOrderApp.Domain.Interfaces;

public interface IReturnRepository
{
    IEnumerable<Return> GetAll();
    void AddReturn(Return returnInfo);
    Return? GetByOrderId(int id);
}

