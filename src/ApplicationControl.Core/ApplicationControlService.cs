using ApplicationControl.Core.Entities;
using ApplicationControl.Core.Extensions;
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
            Status = JobStatus.Queued
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

    public async Task SetQueuedJobStatusAsync(Guid applicationId, Guid commandId, string setBy, JobStatus jobStatus, string message, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(applicationId.ToString(), nameof(applicationId));
        ArgumentException.ThrowIfNullOrEmpty(commandId.ToString(), nameof(commandId));
        ArgumentException.ThrowIfNullOrEmpty(setBy, nameof(setBy));
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));
        ArgumentException.ThrowIfNullOrEmpty(jobStatus.ToString(), nameof(jobStatus));

        await _queuedApplicationJobRepository.SetJobStatusAsync(applicationId, commandId, setBy, jobStatus, message, cancellationToken);
    }
}