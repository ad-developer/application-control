
using ApplicationControl.CommandProcessor;
using ApplicationControl.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace ApplicationControl.Client.Shared;

public class JobRunner : IJobRunner
{
    private readonly SemaphoreSlim _semaphore;
    private readonly ILogger<JobRunner> _logger;
    private readonly ICommandProcessor _commandProcessor;

    public JobRunner(ILogger<JobRunner> logger, ICommandProcessor commandProcessor)
    {
        ArgumentNullException.ThrowIfNull(commandProcessor, nameof(commandProcessor));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
 
        // Initialize the logger
        _logger = logger;

        // Initialize the logger and command processor
        _commandProcessor = commandProcessor;
    
        // Initialize the semaphore with a maximum count of 10
        _semaphore = new SemaphoreSlim(10);
    }

    public async Task RunJobAsync(List<IJob> jobs, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(jobs, nameof(jobs));
     
        var tasks = new List<Task>();

        foreach (var job in jobs)
        {
            _semaphore.Wait(cancellationToken);

            try
            {
                // Run the job asynchronously
                var task = Task.Run(async () =>
                {
                    try
                    {
                        await _commandProcessor.PprocessAsync(job.Command);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error executing job");
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
                _logger.LogError(ex, "Error executing jobs");
            }
            finally
            {
                // Ensure the semaphore is released even if an exception occurs
                _semaphore.Release();
            }
        }

        await Task.WhenAll(tasks);
    }
}
