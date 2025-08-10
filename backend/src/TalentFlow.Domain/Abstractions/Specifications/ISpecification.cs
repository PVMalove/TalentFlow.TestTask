using System.Linq.Expressions;

namespace TalentFlow.Domain.Abstractions.Specifications;

public interface ISpecification<T> where T : class
{
    IReadOnlyList<Expression<Func<T, bool>>> Criteria { get; }
    IReadOnlyList<IncludeExpressionInfo> Includes { get; }
    IReadOnlyList<(Expression<Func<T, object>> KeySelector, bool Descending)> Orderings { get; }
    int? Skip { get; }
    int? Take { get; }
    bool AsNoTracking { get; }
    bool AsSplitQuery { get; }
}