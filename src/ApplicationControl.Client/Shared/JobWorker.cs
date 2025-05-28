using ApplicationControl.Client.Configuration;
using ApplicationControl.CommandProcessor;
using ApplicationControl.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApplicationControl.Client.Shared;

public class JobWorker : IJobWorker
{
    private readonly ILogger<JobWorker> _logger;
    private readonly ICommandProcessor _commandProcessor;
    private readonly IJobsSource _jobsSource;
    private readonly ApplicationControlOptions _options;

    private readonly Guid jobWorkerId = Guid.NewGuid();
    public JobWorker(ILogger<JobWorker> logger, ICommandProcessor commandProcessor, IJobsSource jobsSource, IOptions<ApplicationControlOptions> options)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(commandProcessor, nameof(commandProcessor));
        ArgumentNullException.ThrowIfNull(jobsSource, nameof(jobsSource));
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        _logger = logger;
        _commandProcessor = commandProcessor;
        _jobsSource = jobsSource;
        _options = options.Value;
    }
    public async Task RunJobAsync(IJob job, CancellationToken cancellationToken = default)
    {
         try
        {
            await _jobsSource.SetJobStatusAsync(_options.ApplicaitonId, job.Id, "Job Worker", JobStatus.InProgress, null, cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation($"Executing job {job.Id},  command {job.Command}, jobWorkerId {jobWorkerId}, datetime {DateTime.UtcNow}");

            await _commandProcessor.PprocessAsync(job.Command)
                .ConfigureAwait(false);

            await _jobsSource.SetJobStatusAsync(_options.ApplicaitonId, job.Id, "Job Worker", JobStatus.Completed, null, cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation($"Job {job.Id}, command {job.Command} executed successfully, jobWorkerId {jobWorkerId}, datetime {DateTime.UtcNow}");
        }
        catch (Exception ex)
        {
            await _jobsSource.SetJobStatusAsync(_options.ApplicaitonId, job.Id, "Job Worker", JobStatus.Failed, ex.Message, cancellationToken)
                .ConfigureAwait(false);

            _logger.LogError(ex, $"Error executing job {job.Id}, command {job.Command}, jobWorkerId {jobWorkerId}, datetime {DateTime.UtcNow}");
        }  
    }
}