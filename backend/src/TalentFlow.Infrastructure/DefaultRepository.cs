using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Domain.Abstractions.Repositories;
using TalentFlow.Domain.Abstractions.Specifications;
using TalentFlow.Infrastructure.Specifications;

namespace TalentFlow.Infrastructure
{
    public class DefaultRepository<TEntity>(DbContext dbContext) : IDefaultRepository<TEntity>
        where TEntity : class
    {
        private DbContext context { get; } = dbContext;

        private DbSet<TEntity> DbSet => context.Set<TEntity>();

        public async Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(expression);
            return await DbSet.AnyAsync(expression, cancellationToken);
        }

        public async Task<int> CountAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(expression);
            return await DbSet.CountAsync(expression, cancellationToken);
        }

        public async Task<TEntity?> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(expression);
            return await DbSet.SingleOrDefaultAsync(expression, cancellationToken);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(expression);
            return await DbSet.FirstOrDefaultAsync(expression, cancellationToken);
        }

        public async Task<bool> AnyWithSpecificationAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(specification);

            IQueryable<TEntity> query = DbSet.ApplySpecification(specification);
            return await query.AnyAsync(cancellationToken);
        }

        public async Task<int> CountWithSpecificationAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(specification);

            IQueryable<TEntity> query = DbSet.ApplySpecification(specification);
            return await query.CountAsync(cancellationToken);
        }

        public async Task<TEntity?> SingleOrDefaultWithSpecificationAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(specification);

            IQueryable<TEntity>? query = DbSet.ApplySpecification(specification);
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<TEntity?> FirstOrDefaultWithSpecificationAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(specification);

            IQueryable<TEntity>? query = DbSet.ApplySpecification(specification);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }
    }
}