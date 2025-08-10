using System.Collections;
using System.Linq.Expressions;
using TalentFlow.Domain.Abstractions.Specifications;

namespace TalentFlow.Infrastructure.Specifications;

public sealed class SpecificationBuilder<T> : ISpecificationBuilder<T> where T : class
{
    private readonly BaseSpecification<T> _specification = new Specification<T>();
    private IncludeExpressionInfo? _lastIncludeExpression;

    public ISpecificationBuilder<T> Where(Expression<Func<T, bool>> predicate)
    {
        _specification.AddCriteria(predicate);
        return this;
    }

    public ISpecificationBuilder<T> Include<TProperty>(Expression<Func<T, TProperty>> include)
    {
        var includeInfo = new IncludeExpressionInfo(include);
        _specification.AddInclude(includeInfo);
        _lastIncludeExpression = includeInfo;
        return this;
    }

    public ISpecificationBuilder<T> ThenInclude<TPreviousProperty, TProperty>(
        Expression<Func<TPreviousProperty, TProperty>> thenInclude)
    {
        if (_lastIncludeExpression == null)
        {
            throw new InvalidOperationException(
                "ThenInclude может быть вызван только после Include.");
        }

        Type previousPropertyTypeFromExpression = thenInclude.Parameters[0].Type;
        Type expectedPreviousType = _lastIncludeExpression.LambdaExpression.ReturnType;

        if (typeof(IEnumerable).IsAssignableFrom(expectedPreviousType) &&
            expectedPreviousType.IsGenericType)
        {
            expectedPreviousType = expectedPreviousType.GetGenericArguments()[0];
        }
        
        if (expectedPreviousType != previousPropertyTypeFromExpression)
        {
            throw new InvalidOperationException(
                $"Несоответствие типов в ThenInclude. Ожидался предыдущий тип свойства '{expectedPreviousType.Name}', но получен '{previousPropertyTypeFromExpression.Name}'. " +
                $"Проверьте цепочку вызовов Include/ThenInclude, чтобы убедиться в правильности соответствия типов.");
        }
        
        var thenIncludeInfo = new IncludeExpressionInfo(
            thenInclude, 
            _lastIncludeExpression.LambdaExpression.ReturnType);

        _specification.AddInclude(thenIncludeInfo);
        _lastIncludeExpression = thenIncludeInfo;
        return this;
    }

    public ISpecificationBuilder<T> OrderBy(Expression<Func<T, object>> keySelector)
    {
        _specification.AddOrdering(keySelector, false);
        return this;
    }

    public ISpecificationBuilder<T> OrderByDescending(Expression<Func<T, object>> keySelector)
    {
        _specification.AddOrdering(keySelector, true);
        return this;
    }

    public ISpecificationBuilder<T> Skip(int skip)
    {
        if (skip < 0) throw new ArgumentOutOfRangeException(nameof(skip), "Skip value cannot be negative.");
        _specification.SetSkip(skip);
        return this;
    }

    public ISpecificationBuilder<T> Take(int take)
    {
        if (take < 0) throw new ArgumentOutOfRangeException(nameof(take), "Take value cannot be negative.");
        _specification.SetTake(take);
        return this;
    }

    public ISpecificationBuilder<T> AsNoTracking()
    {
        _specification.SetNoTracking();
        return this;
    }

    public ISpecificationBuilder<T> UseSplitQuery()
    {
        _specification.SetSplitQuery();
        return this;
    }

    public ISpecification<T> Build() => _specification;
}