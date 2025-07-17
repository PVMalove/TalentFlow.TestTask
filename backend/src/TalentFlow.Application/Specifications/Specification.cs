// using System.Linq.Expressions;
//
// namespace TalentFlow.Application.Specifications;
//
// public interface ISpecification<T>
// {
//     IReadOnlyList<Expression<Func<T, bool>>> Criteria { get; }
//     IReadOnlyList<Expression<Func<T, object>>> Includes { get; }
//     bool AsNoTracking { get; }
// }
//
// public class Specification<T> : ISpecification<T>
// {
//     private readonly List<Expression<Func<T, bool>>> _criteriaExpressions = [];
//     public IReadOnlyList<Expression<Func<T, bool>>> Criteria => _criteriaExpressions.AsReadOnly();
//
//     private readonly List<Expression<Func<T, object>>> _includeExpressions = [];
//     public IReadOnlyList<Expression<Func<T, object>>> Includes => _includeExpressions.AsReadOnly();
//
//     public bool AsNoTracking { get; private set; }
//
//     public ISpecificationBuilder<T> Query => new SpecificationBuilder<T>(this);
//
//     public void AddWhere(Expression<Func<T, bool>> expression) => 
//         _criteriaExpressions.Add(expression);
//
//     public void AddInclude(Expression<Func<T, object>> includeExpression) => 
//         _includeExpressions.Add(includeExpression);
//     
//     internal void SetAsNoTracking() => 
//         AsNoTracking = true;
// }
//
//
// public static class SpecificationExtensions
// {
//     public static Specification<T> Where<T>(
//         this Specification<T> specification,
//         Expression<Func<T, bool>> expression)
//     {
//         specification.AddWhere(expression);
//         return specification;
//     }
//
//     public static Specification<T> Include<T>(
//         this Specification<T> specification,
//         Expression<Func<T, object>> includeExpression)
//     {
//         specification.AddInclude(includeExpression);
//         return specification;
//     }
//
//     public static Specification<T> AsNoTracking<T>(
//         this Specification<T> specification)
//     {
//         specification.SetAsNoTracking();
//         return specification;
//     }
// }

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace TalentFlow.Application.Specifications;

public interface ISpecification<T>
{
    // Фильтрация
    IReadOnlyList<Expression<Func<T, bool>>> Criteria { get; }

    // Include и ThenInclude
    IReadOnlyList<Expression<Func<T, object>>> Includes { get; }
    IReadOnlyList<Expression<Func<object, object>>> ThenIncludes { get; }

    // Сортировка: ключ + направление
    IReadOnlyList<(Expression<Func<T, object>> KeySelector, bool Descending)> Orderings { get; }

    // Пагинация
    int? Skip { get; }
    int? Take { get; }
    bool IsPagingEnabled { get; }

    // Без трекинга
    bool AsNoTracking { get; }
}

public class Specification<T> : ISpecification<T>
{
    internal readonly List<Expression<Func<T, bool>>> _criteria = new();
    internal readonly List<Expression<Func<T, object>>> _includes = new();
    internal readonly List<Expression<Func<object, object>>> _thenIncludes = new();
    internal readonly List<(Expression<Func<T, object>>, bool)> _orderings = new();

    public IReadOnlyList<Expression<Func<T, bool>>> Criteria => _criteria;
    public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes;
    public IReadOnlyList<Expression<Func<object, object>>> ThenIncludes => _thenIncludes;
    public IReadOnlyList<(Expression<Func<T, object>>, bool)> Orderings => _orderings;

    public int? Skip { get; private set; }
    public int? Take { get; private set; }
    public bool IsPagingEnabled { get; private set; }
    public bool AsNoTracking { get; private set; }

    // Точка входа для fluent-настройки
    public static SpecificationBuilder<T> Builder() => new SpecificationBuilder<T>(new Specification<T>());

    // Внутренние методы, чтобы Builder мог модифицировать состояние
    internal void AddCriteria(Expression<Func<T, bool>> expr) => _criteria.Add(expr);
    internal void AddInclude(Expression<Func<T, object>> expr) => _includes.Add(expr);
    internal void AddThenInclude(Expression<Func<object, object>> expr) => _thenIncludes.Add(expr);
    internal void AddOrdering(Expression<Func<T, object>> keySelector, bool desc) => _orderings.Add((keySelector, desc));
    internal void EnablePaging(int skip, int take) { Skip = skip; Take = take; IsPagingEnabled = true; }
    internal void EnableNoTracking() => AsNoTracking = true;
}

public class SpecificationBuilder<T>(Specification<T> spec)
{
    public SpecificationBuilder<T> Where(Expression<Func<T, bool>> predicate)
    {
        spec.AddCriteria(predicate);
        return this;
    }

    public SpecificationBuilder<T> Include(Expression<Func<T, object>> include)
    {
        spec.AddInclude(include);
        return this;
    }

    public SpecificationBuilder<T> ThenInclude(Expression<Func<object, object>> thenInclude)
    {
        spec.AddThenInclude(thenInclude);
        return this;
    }

    public SpecificationBuilder<T> OrderBy(Expression<Func<T, object>> keySelector)
    {
        spec.AddOrdering(keySelector, desc: false);
        return this;
    }

    public SpecificationBuilder<T> OrderByDescending(Expression<Func<T, object>> keySelector)
    {
        spec.AddOrdering(keySelector, desc: true);
        return this;
    }

    public SpecificationBuilder<T> Skip(int skip)
    {
        spec.EnablePaging(skip, spec.Take ?? 0);
        return this;
    }

    public SpecificationBuilder<T> Take(int take)
    {
        spec.EnablePaging(spec.Skip ?? 0, take);
        return this;
    }

    public SpecificationBuilder<T> AsNoTracking()
    {
        spec.EnableNoTracking();
        return this;
    }

    public ISpecification<T> Build() => spec;
}

public static class SpecificationEvaluator
{
    public static IQueryable<T> ApplySpecification<T>(
        this IQueryable<T> query,
        ISpecification<T> spec)
        where T : class
    {
        if (spec.AsNoTracking)
            query = query.AsNoTracking();

        foreach (var include in spec.Includes)
            query = query.Include(include);

        foreach (var thenInclude in spec.ThenIncludes)
            query = ((IIncludableQueryable<T, object>)query).ThenInclude(thenInclude);

        foreach (var criterion in spec.Criteria)
            query = query.Where(criterion);

        foreach (var (keySelector, desc) in spec.Orderings)
            query = desc ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);

        if (spec.IsPagingEnabled)
            query = query.Skip(spec.Skip!.Value).Take(spec.Take!.Value);

        return query;
    }
}