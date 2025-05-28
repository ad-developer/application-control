using ApplicationControl.Client.Database.Repositories;
using ApplicationControl.Client.Shared;
using ApplicationControl.Core.Extensions;

namespace ApplicationControl.Client.Database;

public class DatabaseQueuedJobsSource : IJobsSource
{
    private readonly IQueuedApplicationJobRepository _queuedApplicationJobRepository;

public DatabaseQueuedJobsSource(IQueuedApplicationJobRepository queuedApplicationJobRepository)
{
    ArgumentNullException.ThrowIfNull(queuedApplicationJobRepository, nameof(queuedApplicationJobRepository));
    _queuedApplicationJobRepository = queuedApplicationJobRepository;
}
    public async Task<List<IJob>> GetNextJobListAsync(CancellationToken cancellationToken = default)
    {
        return await _queuedApplicationJobRepository.GetNextJobListAsync(cancellationToken);
    }

    public async Task SetJobStatusAsync(Guid applicationId, Guid jobId, string setBy, JobStatus jobStatus, string? message, CancellationToken cancellationToken = default)
    {
        await _queuedApplicationJobRepository.SetJobStatusAsync(applicationId, jobId, setBy, jobStatus, message, cancellationToken);
    }
}
