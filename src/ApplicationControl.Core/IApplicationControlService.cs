using ApplicationControl.Core.Entities;
using ApplicationControl.Core.Extensions;

namespace ApplicationControl.Core;

public interface IApplicationControlService
{
    Task<QueuedApplicationJob?> GetQueuedJobAsync(Guid applicationId, CancellationToken cancellationToken = default);
    Task SetQueuedJobStatusAsync(Guid applicationId, Guid commandId, string setBy, JobStatus jobStatus, string message, CancellationToken cancellationToken = default); 
    Task<QueuedApplicationJob> QueueQueuedJobAsync(Guid applicaitonId, string command,  string addedBy, CancellationToken cancellationToken = default);
}
