using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SilverDapper.Abstractions;

internal class DbConnectionCheckoutService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<DbConnectionCheckoutService> logger;

    private PeriodicTimer timer = null!;

    public DbConnectionCheckoutService(IServiceProvider serviceProvider, ILogger<DbConnectionCheckoutService> logger)
    {
        this.serviceProvider = serviceProvider;
        this.logger = logger;
    }

    public override void Dispose()
    {
        timer = null!;
        base.Dispose();
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        timer = new PeriodicTimer(TimeSpan.FromHours(1));
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IDapperContext>();

            try
            {
                using var connection = await context.GetConnectionAsync();
                connection.Close();
            }
            catch (DbException ex)
            {
                logger.LogError(ex, "Errors during testing connection");
            }
        }
    }
}