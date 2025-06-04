using System.Data.Common;
using Microsoft.Data.SqlClient;
using TalentFlow.Application.Abstractions;

namespace TalentFlow.Infrastructure;

internal sealed class DbConnectionFactory(string? connectionString) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
    }
}