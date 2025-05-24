
using ApplicationControl.Client.Configuration;
using ApplicationControl.Client.Database.Repositories;
using ApplicationControl.CommandProcessor;
using ApplicationControl.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApplicationControl.Client.Shared;

public class JobRunner : IJobRunner
{
    private readonly SemaphoreSlim _semaphore;
    private readonly ILogger<JobRunner> _logger;
    private readonly ICommandProcessor _commandProcessor;
    private readonly IQueuedApplicationJobRepository _queuedApplicationJobRepository;

    private readonly ApplicationControlOptions _options;

    public JobRunner(ILogger<JobRunner> logger, ICommandProcessor commandProcessor, IQueuedApplicationJobRepository queuedApplicationJobRepository, IOptions<ApplicationControlOptions> options)
    {
        ArgumentNullException.ThrowIfNull(commandProcessor, nameof(commandProcessor));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(queuedApplicationJobRepository, nameof(queuedApplicationJobRepository));
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        // Initialize the logger
        _logger = logger;

        // Initialize the logger and command processor
        _commandProcessor = commandProcessor;

        // Initialize the queued application job repository
        _queuedApplicationJobRepository = queuedApplicationJobRepository;

        // Initialize the options
        _options = options.Value;

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
                    await _queuedApplicationJobRepository.SetJobStatusAsync(_options.ApplicaitonId, job.Id, "Job Runner", JobStatus.InProgress, null, cancellationToken)
                        .ConfigureAwait(false);

                    _logger.LogInformation($"Executing job {job.Id} with command {job.Command}, jobRunnerId {jobRunnerId}, datetime {DateTime.UtcNow}");

                    await _commandProcessor.PprocessAsync(job.Command)
                        .ConfigureAwait(false);

                    await _queuedApplicationJobRepository.SetJobStatusAsync(_options.ApplicaitonId, job.Id, "Job Runner", JobStatus.Completed, null, cancellationToken)
                        .ConfigureAwait(false);

                    _logger.LogInformation($"Job {job.Id} with command {job.Command} executed successfully, jobRunnerId {jobRunnerId}, datetime {DateTime.UtcNow}");
                }
                catch (Exception ex)
                {
                    await _queuedApplicationJobRepository.SetJobStatusAsync(_options.ApplicaitonId, job.Id, "Job Runner", JobStatus.Failed, ex.Message, cancellationToken)
                        .ConfigureAwait(false);

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
