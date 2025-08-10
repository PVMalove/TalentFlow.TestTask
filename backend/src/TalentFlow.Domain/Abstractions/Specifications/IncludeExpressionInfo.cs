using System.Linq.Expressions;

namespace TalentFlow.Domain.Abstractions.Specifications;

public enum IncludeTypeEnum
{
    Include = 1,
    ThenInclude = 2
}

public class IncludeExpressionInfo
{
    public LambdaExpression LambdaExpression { get; }
    public Type? PreviousPropertyType { get; }
    public IncludeTypeEnum Type { get; }

    public IncludeExpressionInfo(LambdaExpression expression)
    {
        LambdaExpression = expression ?? throw new ArgumentNullException(nameof(expression));
        PreviousPropertyType = null;
        Type = IncludeTypeEnum.Include;
    }

    public IncludeExpressionInfo(LambdaExpression expression, Type previousPropertyType)
    {
        LambdaExpression = expression ?? throw new ArgumentNullException(nameof(expression));
        PreviousPropertyType = previousPropertyType ?? throw new ArgumentNullException(nameof(previousPropertyType));
        Type = IncludeTypeEnum.ThenInclude;
    }
}