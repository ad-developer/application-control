using ApplicationControl.Client.Shared;
using ApplicationControl.Core.Extensions;

namespace ApplicationControl.Client.Api;

public class ApiQueuedJobsHostedSource : IJobsSource
{
    public Task<List<IJob>> GetNextJobListAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SetJobStatusAsync(Guid applicationId, Guid jobId, string setBy, JobStatus jobStatus, string? message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
