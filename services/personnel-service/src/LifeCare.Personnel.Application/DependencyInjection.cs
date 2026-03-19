using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace LifeCare.Personnel.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // 1. Register all Validators from this assembly (FluentValidation)
        // This finds UpdatePersonnelCommandValidator automatically
        services.AddValidatorsFromAssembly(assembly);

        // 2. Register MediatR
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            
            // 3. Register the Validation "Gatekeeper"
            // This ensures every Command is validated before the Handler runs
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}