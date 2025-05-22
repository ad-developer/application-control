using ApplicationControl.Core.Entities;
using ApplicationControl.Core.Extensions;

namespace ApplicationControl.Core.Respositories;

public interface IQueuedApplicationJobRepository : IBaseRepository<QueuedApplicationJob, Guid>
{
    Task<QueuedApplicationJob?> GetNextJobAsync(CancellationToken cancellationToken);
    Task SetJobStatusAsync(Guid applicationId, Guid commandId, string setBy,JobStatus queuedJobStatus, string message, CancellationToken cancellationToken);
}
