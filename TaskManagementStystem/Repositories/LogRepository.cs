using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementStystem.Models;

public class LogRepository
{
    private readonly string _connectionString;

    public LogRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<(IEnumerable<LogModel> logs, int totalRecords)> GetLogsAsync(int page = 1, int pageSize = 10)
    {
        using var connection = new SqlConnection(_connectionString);

        string query = @"
        WITH LogCTE AS (
            SELECT 
                Id, TimeStamp, Message, MessageTemplate, 
                COUNT(*) OVER() AS TotalRecords
            FROM Logs
        )
        SELECT * FROM LogCTE
        ORDER BY TimeStamp DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        var logs = await connection.QueryAsync<LogModel>(query, new
        {
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });

        int totalRecords = logs.Any() ? logs.First().TotalRecords : 0;

        return (logs, totalRecords);
    }
}
