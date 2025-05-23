
using ApplicationControl.Client.Database.Repositories;
using ApplicationControl.Client.Shared;
using Microsoft.Extensions.Logging;

namespace ApplicationControl.Client.Database.HostedServices;

public class DatabaseQueuedJobWorker : IDatabaseQueuedJobWorker
{
    private readonly IQueuedApplicationJobRepository _queuedApplicationJobRepository;
    private readonly ILogger<DatabaseQueuedJobWorker> _logger;
    private readonly IJobRunner _jobRunner;

    public DatabaseQueuedJobWorker(
        IQueuedApplicationJobRepository queuedApplicationJobRepository,
        ILogger<DatabaseQueuedJobWorker> logger,
        IJobRunner jobRunner)
    {
        _queuedApplicationJobRepository = queuedApplicationJobRepository;
        _logger = logger;
        _jobRunner = jobRunner;
    }

    public async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"QueueProcessingWorker started, time {DateTime.UtcNow}");

        var jobList = await _queuedApplicationJobRepository.GetNextJobListAsync(cancellationToken);

        if (jobList.Count == 0)
        {
            _logger.LogInformation($"QueueProcessingWorker found no jobs to process, time {DateTime.UtcNow}");
            return;
        }

        _logger.LogInformation($"QueueProcessingWorker found {jobList.Count} jobs to process, time {DateTime.UtcNow}");

        await _jobRunner.RunJobAsync(jobList, cancellationToken);

        _logger.LogInformation($"QueueProcessingWorker completed, time {DateTime.UtcNow}");
    }
}
