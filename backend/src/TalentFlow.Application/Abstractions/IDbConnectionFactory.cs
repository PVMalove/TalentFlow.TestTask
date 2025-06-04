using System.Data.Common;

namespace TalentFlow.Application.Abstractions;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}