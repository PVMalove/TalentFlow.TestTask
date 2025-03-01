using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using TalentFlow.Application.Abstractions.Common;

namespace TalentFlow.Infrastructure.Common;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}