using Microsoft.Extensions.DependencyInjection;

namespace SilverDapper.Abstractions.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDapper<TContext>(this IServiceCollection services,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        where TContext : class, IDapperContext
    {
        services.AddSingleton<IDapperCache, DapperCache>();
        services.AddHostedService<DbConnectionCheckoutService>();
        services.Add(new ServiceDescriptor(typeof(IDapperContext), typeof(TContext), contextLifetime));

        return services;
    }
}