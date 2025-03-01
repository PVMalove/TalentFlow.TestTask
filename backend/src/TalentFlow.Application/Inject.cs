using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TalentFlow.Application.Abstractions;
using TalentFlow.Application.Validation;


namespace TalentFlow.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(c => c
            .RegisterServicesFromAssembly(typeof(Inject).Assembly)
            .AddOpenBehavior(typeof(ResultValidationPipelineBehavior<,>)));

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        services.AddHandlers();

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection collection)
    {
        collection.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        collection.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return collection;
    }
}