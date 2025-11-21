using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;

namespace MiniOrderApp.Infrastructure.Repositories;

public class ReturnRepository : IReturnRepository
{
    private readonly SQLiteConnectionFactory _factory;
    public ReturnRepository(SQLiteConnectionFactory factory)
    {
        _factory = factory;
    }
    public void AddReturn(Return returnInfo)
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
        INSERT INTO Returns (OrderId, ReturnDate, Reason, RefundedAmount)
        VALUES (@OrderId, @ReturnDate, @Reason, @RefundedAmount);";

        db.Execute(sql, new
        {
            returnInfo.OrderId,
            ReturnDate = returnInfo.ReturnDate.ToString("yyyy-MM-dd"),
            returnInfo.Reason,
            returnInfo.RefundedAmount
        });
    }

    public Return? GetByOrderId(int orderId)
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
        SELECT
            ReturnId       AS Id,
            OrderId,
            ReturnDate,
            Reason,
            RefundedAmount
        FROM Returns
        WHERE OrderId = @OrderId;
        ";

        return db.QueryFirstOrDefault<Return>(sql, new { OrderId = orderId });
    }
}
