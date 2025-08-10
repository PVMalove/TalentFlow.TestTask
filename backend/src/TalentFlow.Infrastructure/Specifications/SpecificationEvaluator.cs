using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Domain.Abstractions.Specifications;

namespace TalentFlow.Infrastructure.Specifications;

public static class SpecificationEvaluator
{
    /// <summary>
    /// Применяет спецификацию к IQueryable для получения сущностей.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <param name="query">Исходный IQueryable.</param>
    /// <param name="spec">Применяемая спецификация.</param>
    /// <returns>Модифицированный IQueryable с примененными фильтрами, включениями, сортировкой и пагинацией.</returns>
    public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class
    {
        if (spec.AsNoTracking) 
            query = query.AsNoTracking();

        if (spec.AsSplitQuery) 
            query = query.AsSplitQuery();

        foreach (Expression<Func<T, bool>> criteria in spec.Criteria)
        {
            query = query.Where(criteria);
        }

        query = IncludeEvaluator.Instance.ApplyIncludes(query, spec);

        if (spec.Orderings.Count > 0)
        {
            (Expression<Func<T, object>> KeySelector, bool Descending) firstOrdering = spec.Orderings[0];
            IOrderedQueryable<T> orderedQuery = firstOrdering.Descending
                ? query.OrderByDescending(firstOrdering.KeySelector)
                : query.OrderBy(firstOrdering.KeySelector);

            for (int i = 1; i < spec.Orderings.Count; i++)
            {
                (Expression<Func<T, object>> keySelector, bool descending) = spec.Orderings[i];
                orderedQuery = descending
                    ? orderedQuery.ThenByDescending(keySelector)
                    : orderedQuery.ThenBy(keySelector);
            }
            query = orderedQuery;
        }

        if (spec.Skip.HasValue)
            query = query.Skip(spec.Skip.Value);

        if (spec.Take.HasValue)
            query = query.Take(spec.Take.Value);

        return query;
    }
}