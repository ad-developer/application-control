using ApplicationControl.Client.Configuration;
using ApplicationControl.Client.Database.HostedServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApplicationControl.Client.Database.hostedServices;

public class DatabaseQueuedJobHostedService(IServiceProvider services, ILogger<DatabaseQueuedJobHostedService> logger, IOptions<Settings> options) : BackgroundService
{
    private readonly ILogger<DatabaseQueuedJobHostedService> _logger = logger;
    private readonly Settings _options = options.Value;
    public IServiceProvider Services { get; } = services;
    Guid _processId = Guid.NewGuid();
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DoWorkAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                var delayInSeconds = _options.QueuedHastedServiceResetProcessCycle * 1000;
                _logger.LogError(ex, $"An error occurred while executing the DatabaseQueuedJobHostedService. Service will be restarted in {delayInSeconds} seconds.");
                
                Task.Delay(delayInSeconds, stoppingToken).Wait(stoppingToken);
            }
        }
    }
    internal async Task DoWorkAsync(CancellationToken cancellationToken)
    {
         _logger.LogInformation($"QueueProcessingHostedService started, process id {_processId}, time {DateTime.UtcNow}"); 
        int cycleCount = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            cycleCount++;
            Guid cycleId = Guid.NewGuid();
            try
            {
                using (var scope = Services.CreateScope())
                {
                    _logger.LogInformation($"QueueProcessingHostedService started new cycle, number {cycleCount}, process id {_processId}, scope id {cycleId}, ime {DateTime.UtcNow}");

                    var worker =
                        scope.ServiceProvider
                            .GetRequiredService<IDatabaseQueuedJobWorker>();

                    await worker.DoWorkAsync(cancellationToken);

                    _logger.LogInformation($"QueueProcessingHostedService completed new cycle, number {cycleCount}, process id {_processId}, scope id {cycleId}, time {DateTime.UtcNow}");
                }

                var delayInSeconds = _options.QueuedHastedServiceCycle * 1000;
                await Task.Delay(delayInSeconds, cancellationToken);
            }
            catch (Exception ex)
            {
                var delayInSeconds = _options.QueuedHastedResetServiceCycle * 1000;
                _logger.LogError(ex, $"QueueProcessingHostedService failed cycle, number {cycleCount}, process id {_processId}, scope id {cycleId}, time {DateTime.UtcNow}. Cycle will be restarted in {delayInSeconds} seconds.");

                await Task.Delay(delayInSeconds, cancellationToken);
            }
        }
    }
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"QueueProcessingHostedService is stopping, process id {_processId} time {DateTime.UtcNow}");
        await base.StopAsync(cancellationToken);
    }
}