using System.Linq.Expressions;
using TalentFlow.Domain.Abstractions.Specifications;
using TalentFlow.Domain.Shared;
using ComparableValueObject = CSharpFunctionalExtensions.ComparableValueObject;

namespace TalentFlow.Domain.Abstractions.Repositories
{
    public interface IDefaultRepository<TEntity> where TEntity : class
    {
        Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default);

        Task<TEntity?> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default);

        Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default);
        
        Task<TEntity?> SingleOrDefaultWithSpecificationAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default);

        Task<TEntity?> FirstOrDefaultWithSpecificationAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default);

        Task<bool> AnyWithSpecificationAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default);

        Task<int> CountWithSpecificationAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default);
    }
}