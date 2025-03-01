using FluentValidation;
using TalentFlow.Domain.Shared;


namespace TalentFlow.Application.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, Error error) => rule.WithMessage(error.Serialize());
}