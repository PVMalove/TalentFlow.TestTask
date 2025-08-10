using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TalentFlow.Domain.Abstractions.Specifications;

namespace TalentFlow.Infrastructure.Specifications;

public class IncludeEvaluator
{
    private static readonly MethodInfo _includeMethodInfo = typeof(EntityFrameworkQueryableExtensions)
        .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.Include))
        .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 2
                                  && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() ==
                                  typeof(IQueryable<>)
                                  && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() ==
                                  typeof(Expression<>));

    private static readonly MethodInfo _thenIncludeAfterReferenceMethodInfo
        = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
            .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 3
                                      && mi.GetParameters()[0].ParameterType.GetGenericArguments()[1].IsGenericParameter
                                      && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() ==
                                      typeof(IIncludableQueryable<,>)
                                      && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() ==
                                      typeof(Expression<>));

    private static readonly MethodInfo _thenIncludeAfterEnumerableMethodInfo
        = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
            .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 3
                                      && !mi.GetParameters()[0].ParameterType.GetGenericArguments()[1]
                                          .IsGenericParameter
                                      && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() ==
                                      typeof(IIncludableQueryable<,>)
                                      && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() ==
                                      typeof(Expression<>));

    private readonly record struct CacheKey(Type EntityType, Type PropertyType, Type? PreviousPropertyType);

    private static readonly ConcurrentDictionary<CacheKey, Func<IQueryable, LambdaExpression, IQueryable>> _cache =
        new();

    private IncludeEvaluator()
    { }

    public static readonly IncludeEvaluator Instance = new();
    
    public IQueryable<T> ApplyIncludes<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (!specification.Includes.Any()) return query;

        IQueryable currentQuery = query;

        foreach (IncludeExpressionInfo includeExpression in specification.Includes)
        {
            LambdaExpression lambdaExpr = includeExpression.LambdaExpression;

            switch (includeExpression.Type)
            {
                case IncludeTypeEnum.Include:
                {
                    CacheKey key = new CacheKey(typeof(T), lambdaExpr.ReturnType, null);
                    Func<IQueryable, LambdaExpression, IQueryable> includeDelegate = _cache.GetOrAdd(key, CreateIncludeDelegate);
                    currentQuery = includeDelegate(currentQuery, lambdaExpr);
                    break;
                }
                case IncludeTypeEnum.ThenInclude:
                {
                    Debug.Assert(includeExpression.PreviousPropertyType is not null);
                    CacheKey key = new CacheKey(typeof(T), lambdaExpr.ReturnType, includeExpression.PreviousPropertyType);
                    Func<IQueryable, LambdaExpression, IQueryable> thenIncludeDelegate = _cache.GetOrAdd(key, CreateThenIncludeDelegate);
                    currentQuery = thenIncludeDelegate(currentQuery, lambdaExpr);
                    break;
                }
            }
        }

        return (IQueryable<T>)currentQuery;
    }

    private static Func<IQueryable, LambdaExpression, IQueryable> CreateIncludeDelegate(CacheKey cacheKey)
    {
        MethodInfo includeMethod = _includeMethodInfo.MakeGenericMethod(cacheKey.EntityType, cacheKey.PropertyType);

        ParameterExpression sourceParameter = Expression.Parameter(typeof(IQueryable), "source");
        ParameterExpression selectorParameter = Expression.Parameter(typeof(LambdaExpression), "selector");

        MethodCallExpression call = Expression.Call(
            includeMethod,
            Expression.Convert(sourceParameter, typeof(IQueryable<>).MakeGenericType(cacheKey.EntityType)),
            Expression.Convert(selectorParameter,
                typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(cacheKey.EntityType,
                    cacheKey.PropertyType)))
        );

        Expression<Func<IQueryable, LambdaExpression, IQueryable>> lambda =
            Expression.Lambda<Func<IQueryable, LambdaExpression, IQueryable>>(call, sourceParameter, selectorParameter);
        return lambda.Compile();
    }

    private static Func<IQueryable, LambdaExpression, IQueryable> CreateThenIncludeDelegate(CacheKey cacheKey)
    {
        Debug.Assert(cacheKey.PreviousPropertyType is not null);

        MethodInfo thenIncludeInfo = IsGenericEnumerable(cacheKey.PreviousPropertyType, out var previousElementType)
            ? _thenIncludeAfterEnumerableMethodInfo
            : _thenIncludeAfterReferenceMethodInfo;

        MethodInfo thenIncludeMethod =
            thenIncludeInfo.MakeGenericMethod(cacheKey.EntityType, previousElementType, cacheKey.PropertyType);

        ParameterExpression sourceParameter = Expression.Parameter(typeof(IQueryable), "source");
        ParameterExpression selectorParameter = Expression.Parameter(typeof(LambdaExpression), "selector");

        Type includableQueryableType =
            typeof(IIncludableQueryable<,>).MakeGenericType(cacheKey.EntityType, cacheKey.PreviousPropertyType);

        MethodCallExpression call = Expression.Call(
            thenIncludeMethod,
            Expression.Convert(sourceParameter, includableQueryableType),
            Expression.Convert(selectorParameter,
                typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(previousElementType,
                    cacheKey.PropertyType)))
        );

        Expression<Func<IQueryable, LambdaExpression, IQueryable>> lambda =
            Expression.Lambda<Func<IQueryable, LambdaExpression, IQueryable>>(call, sourceParameter, selectorParameter);
        return lambda.Compile();
    }

    private static bool IsGenericEnumerable(Type type, out Type elementType)
    {
        Type? enumerableType = type.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        if (enumerableType != null)
        {
            elementType = enumerableType.GenericTypeArguments[0];
            return true;
        }

        elementType = type;
        return false;
    }
}