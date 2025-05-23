
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
        var jobRunnerId = Guid.NewGuid();
        _logger.LogInformation($"JobRunner started, tatal jobs number {jobs.Count},  jobRunnerId {jobRunnerId}, time {DateTime.UtcNow}");

        foreach (var job in jobs)
        {
            _semaphore.Wait(cancellationToken);
            
            // Run the job asynchronously
            var task = Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation($"Executing job {job.Id} with command {job.Command}, jobRunnerId {jobRunnerId}, datetime {DateTime.UtcNow}");

                    await _commandProcessor.PprocessAsync(job.Command);

                    _logger.LogInformation($"Job {job.Id} with command {job.Command} executed successfully, jobRunnerId {jobRunnerId}, datetime {DateTime.UtcNow}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error executing job {job.Id} with command {job.Command}, jobRunnerId {jobRunnerId}, datetime {DateTime.UtcNow}");
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
         
        _logger.LogInformation($"JobRunner completed, jobRunnerId {jobRunnerId}, time {DateTime.UtcNow}");
    }
}
