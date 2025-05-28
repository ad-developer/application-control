
using Microsoft.Extensions.Logging;

namespace ApplicationControl.Client.Shared;

public class JobsListener : IJobsListener
{
    private readonly IJobsSource _jobsSource;
    private readonly ILogger<JobsListener> _logger;
    private readonly IJobsRunner _jobsRunner;

    public JobsListener(
        IJobsSource jobsSource,
        ILogger<JobsListener> logger,
        IJobsRunner jobsRunner)
    {
        ArgumentNullException.ThrowIfNull(jobsSource, nameof(jobsSource));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(jobsRunner, nameof(jobsRunner));

        _jobsSource = jobsSource;
        _logger = logger;
        _jobsRunner = jobsRunner;
    }
    public async Task ListenAsync(CancellationToken cancellationToken = default)
    {
         _logger.LogInformation($"JobsListener started, time {DateTime.UtcNow}");

        var jobList = await _jobsSource.GetNextJobListAsync(cancellationToken);

        if (jobList.Count == 0)
        {
            _logger.LogInformation($"JobsListener found no jobs to process, time {DateTime.UtcNow}");
            return;
        }

        _logger.LogInformation($"JobsListener found {jobList.Count} jobs to process, time {DateTime.UtcNow}");

        await _jobsRunner.RunJobsAsync(jobList, cancellationToken);

        _logger.LogInformation($"JobsListener completed, time {DateTime.UtcNow}");
    }
}
