using ApplicationControl.Client.Configuration;
using ApplicationControl.Client.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApplicationControl.Client;

public class QueuedJobsHostedService(IServiceProvider services, ILogger<QueuedJobsHostedService> logger, IOptions<ApplicationControlOptions> options) : BackgroundService
{
    private readonly ILogger<QueuedJobsHostedService> _logger = logger;
    private readonly ApplicationControlOptions _options = options.Value;
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
                _logger.LogError(ex, $"An error occurred while executing the QueuedJobsHostedService. Service will be restarted in {delayInSeconds} seconds.");
                
                Task.Delay(delayInSeconds, stoppingToken).Wait(stoppingToken);
            }
        }
    }
    internal async Task DoWorkAsync(CancellationToken cancellationToken)
    {
         _logger.LogInformation($"QueuedJobsHostedService started, process id {_processId}, datetime {DateTime.UtcNow}"); 
        int cycleCount = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            cycleCount++;
            Guid cycleId = Guid.NewGuid();
            try
            {
                using (var scope = Services.CreateScope())
                {
                    _logger.LogInformation($"QueuedJobsHostedService started new cycle, number {cycleCount}, process id {_processId}, cycle id {cycleId}, datetime {DateTime.UtcNow}");

                    var listner =
                        scope.ServiceProvider
                            .GetRequiredService<IJobsListener>();

                    await listner.ListenAsync(cancellationToken);

                    _logger.LogInformation($"QueuedJobsHostedService completed new cycle, number {cycleCount}, process id {_processId}, cycle id {cycleId}, datetime {DateTime.UtcNow}");
                }

                var delayInSeconds = _options.QueuedHastedServiceCycle * 1000;
                await Task.Delay(delayInSeconds, cancellationToken);
            }
            catch (Exception ex)
            {
                var delayInSeconds = _options.QueuedHastedResetServiceCycle * 1000;
                _logger.LogError(ex, $"QueuedJobsHostedService failed, cycle number {cycleCount}, process id {_processId}, cycle id {cycleId}, datetime {DateTime.UtcNow}. Cycle will restart in {delayInSeconds} seconds.");

                await Task.Delay(delayInSeconds, cancellationToken);
            }
        }
    }
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"QueuedJobsHostedService is stopping, process id {_processId} datetime {DateTime.UtcNow}");
        await base.StopAsync(cancellationToken);
    }
}