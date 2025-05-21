
using Microsoft.Extensions.Logging;

namespace ApplicationControl.Client.Shared;

public class JobRunner : IJobRunner
{
    private readonly SemaphoreSlim _semaphore;
    private readonly ILogger<JobRunner> _logger;

    public JobRunner(ILogger<JobRunner> logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        // Initialize the logger
        _logger = logger;

        // Initialize the semaphore with a maximum count of 10
        _semaphore = new SemaphoreSlim(10);
    }
    public async Task RunJobAsync(List<IJob> jobs, Action<IJob> action, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(jobs, nameof(jobs));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        var tasks = new List<Task>();

        foreach (var job in jobs)
        {
            _semaphore.Wait(cancellationToken);
            try
            {
                // Run the job asynchronously
                var task = Task.Run(() =>
                {
                    try
                    {
                         action(job);
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions as needed
                        _logger.LogError(ex, "Error executing job: {Message}", ex.Message);
                    }
                    finally
                    {
                        // Release the semaphore when the job is done
                        _semaphore.Release();
                    }
                }, cancellationToken);
                tasks.Add(task);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                _logger.LogError(ex, "Error executing job: {Message}", ex.Message);

                _semaphore.Release();
            }
        }

        await Task.WhenAll(tasks);
    }
}
