using ApplicationControl.Client.Configuration;
using ApplicationControl.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApplicationControl.Client.Shared;

public class JobsRunner : IJobsRunner
{
    private readonly SemaphoreSlim _semaphore;
    private readonly ILogger<JobsRunner> _logger;
    private readonly ApplicationControlOptions _options;
    private readonly IServiceProvider _services; 
    public JobsRunner(ILogger<JobsRunner> logger, IOptions<ApplicationControlOptions> options, IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        // Initialize the logger
        _logger = logger;

        // Initialize the options
        _options = options.Value;

        // Initialize the service provider
        _services = services;
    
        var maxJobsCount = _options.MaxJobsCount?? 10;
        _semaphore = new SemaphoreSlim(maxJobsCount) ; 
    }
    public async Task RunJobsAsync(List<IJob> jobs, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(jobs, nameof(jobs));

        var tasks = new List<Task>();
        var jobRunnerId = Guid.NewGuid();
        _logger.LogInformation($"JobsRunner started, tatal jobs number {jobs.Count},  jobRunnerId {jobRunnerId}, datetime {DateTime.UtcNow}");

        foreach (var job in jobs)
        {
            _semaphore.Wait(cancellationToken);
            
            // Run the job asynchronously
            var task = Task.Run(async () =>
            {
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var worker =
                        scope.ServiceProvider
                            .GetRequiredService<IJobWorker>();

                        await worker.RunJobAsync(job, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error executing job worker, jobId {job.Id}, command {job.Command}, jobRunnerId {jobRunnerId}, datetime {DateTime.UtcNow}");
                }
                finally
                {
                    // Release the semaphore when the job is done
                    _semaphore.Release();
                }
            }, cancellationToken);

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
         
        _logger.LogInformation($"JobsRunner completed, jobRunnerId {jobRunnerId}, datetime {DateTime.UtcNow}");
    }
}
