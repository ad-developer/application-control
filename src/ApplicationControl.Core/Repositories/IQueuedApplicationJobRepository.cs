using ApplicationControl.Core.Common;
using ApplicationControl.Core.Entities;

namespace ApplicationControl.Core.Respositories;

public interface IQueuedApplicationJobRepository : IBaseRepository<QueuedApplicationJob, Guid>
{
    Task<QueuedApplicationJob?> GetNextJobAsync(CancellationToken cancellationToken);
    Task SetJobStatusAsync(Guid applicationId, Guid commandId, string setBy,QueuedJobStatus queuedJobStatus, string message, CancellationToken cancellationToken);
}
