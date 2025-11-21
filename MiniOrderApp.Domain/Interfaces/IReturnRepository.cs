namespace MiniOrderApp.Domain.Interfaces;

public interface IReturnRepository
{
    void AddReturn(Return returnInfo);
    Return GetByOrderId(int id);
}

