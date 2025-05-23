using ApplicationControl.Client.Database.Entities;
using ApplicationControl.Core.Extensions;

namespace ApplicationControl.Client.Database.Repositories;

public interface IQueuedApplicationJobRepository : IBaseRepository<QueuedApplicationJob, Guid>
{
    Task<List<IJob>> GetNextJobListAsync(CancellationToken cancellationToken);

    Task SetJobStatusAsync(Guid applicationId, Guid jobId, string setBy, JobStatus jobStatus, string? message, CancellationToken cancellationToken);
}
