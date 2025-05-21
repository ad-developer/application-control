using ApplicationControl.Core.Common;
using ApplicationControl.Core.Entities;
using ApplicationControl.Core.Respositories;

namespace ApplicationControl.Core;

public class ApplicationControlService(IQueuedApplicationJobRepository queuedApplicationJobRepository) : IApplicationControlService
{
    private readonly IQueuedApplicationJobRepository _queuedApplicationJobRepository = queuedApplicationJobRepository;

    public async Task<QueuedApplicationJob> QueueQueuedJobAsync(Guid applicaitonId, string command,  string addedBy, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(applicaitonId.ToString(), nameof(applicaitonId));
        ArgumentException.ThrowIfNullOrEmpty(command, nameof(command));
        ArgumentException.ThrowIfNullOrEmpty(addedBy, nameof(addedBy));
        
        var applicationJob = new QueuedApplicationJob
        {
            ApplicationId = applicaitonId,
            Command = command,
            Status = QueuedJobStatus.Queued
        };
        
        var res = await _queuedApplicationJobRepository.AddAsync(applicationJob, addedBy, cancellationToken);
        return res;
    }

    public async Task<QueuedApplicationJob?> GetQueuedJobAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(applicationId.ToString(), nameof(applicationId));

        var nextCommand = await _queuedApplicationJobRepository.GetNextJobAsync(cancellationToken);

        return nextCommand;
    }

    public async Task SetQueuedJobStatusAsync(Guid applicationId, Guid commandId, string setBy, QueuedJobStatus queuedJobStatus, string message, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(applicationId.ToString(), nameof(applicationId));
        ArgumentException.ThrowIfNullOrEmpty(commandId.ToString(), nameof(commandId));
        ArgumentException.ThrowIfNullOrEmpty(setBy, nameof(setBy));
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));
        ArgumentException.ThrowIfNullOrEmpty(queuedJobStatus.ToString(), nameof(queuedJobStatus));

        await _queuedApplicationJobRepository.SetJobStatusAsync(applicationId, commandId, setBy, queuedJobStatus, message, cancellationToken);
    }
}