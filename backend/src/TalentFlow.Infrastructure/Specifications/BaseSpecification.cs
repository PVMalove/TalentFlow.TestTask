using System.Linq.Expressions;
using TalentFlow.Domain.Abstractions.Specifications;

namespace TalentFlow.Infrastructure.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T> where T : class
{
    private readonly List<Expression<Func<T, bool>>> _criteria = [];
    private readonly List<IncludeExpressionInfo> _includes = [];
    private readonly List<(Expression<Func<T, object>> KeySelector, bool Descending)> _orderings = [];

    public IReadOnlyList<Expression<Func<T, bool>>> Criteria => _criteria.AsReadOnly();
    public IReadOnlyList<IncludeExpressionInfo> Includes => _includes.AsReadOnly();

    public IReadOnlyList<(Expression<Func<T, object>> KeySelector, bool Descending)> Orderings =>
        _orderings.AsReadOnly();

    public int? Skip { get; private set; }
    public int? Take { get; private set; }
    public bool AsNoTracking { get; private set; }
    public bool AsSplitQuery { get; private set; }

    protected internal void AddCriteria(Expression<Func<T, bool>> predicate) => _criteria.Add(predicate);
    protected internal void AddInclude(IncludeExpressionInfo include) => _includes.Add(include);

    protected internal void AddOrdering(Expression<Func<T, object>> keySelector, bool descending) =>
        _orderings.Add((keySelector, descending));

    protected internal void SetSkip(int skip) => Skip = skip;
    protected internal void SetTake(int take) => Take = take;
    protected internal void SetNoTracking() => AsNoTracking = true;
    protected internal void SetSplitQuery() => AsSplitQuery = true;
}

public class Specification<T> : BaseSpecification<T> where T : class
{ }