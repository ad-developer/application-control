using ApplicationControl.Core.Extensions;

namespace ApplicationControl.Client.Shared;

public interface IJobsSource
{
    Task<List<IJob>> GetNextJobListAsync(CancellationToken cancellationToken = default);
    Task SetJobStatusAsync(Guid applicationId, Guid jobId, string setBy, JobStatus jobStatus, string? message, CancellationToken cancellationToken= default);
}