using Microsoft.Extensions.DependencyInjection;
using SilverDapper.Abstractions.Extensions;

namespace SilverDapper.SqlClient.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqlClient(this IServiceCollection services, Action<SqlClientOptions> configuration,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        var options = new SqlClientOptions();
        configuration.Invoke(options);

        services.AddSingleton(options);
        services.AddDapper<SqlClientContext>(contextLifetime);

        return services;
    }

    public static IServiceCollection AddSqlClient(this IServiceCollection services, Action<IServiceProvider, SqlClientOptions> configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        services.AddScoped(provider =>
        {
            var options = new SqlClientOptions();
            configuration.Invoke(provider, options);

            return options;
        });

        services.AddDapper<SqlClientContext>(ServiceLifetime.Scoped);
        return services;
    }
}