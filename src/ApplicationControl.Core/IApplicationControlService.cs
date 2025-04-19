using ApplicationControl.Core.Common;


namespace ApplicationControl.Core;

public interface IApplicationControlService
{
    Task<CommandQueueItem?> GetNextCommandAsync(Guid applicationId, CancellationToken cancellationToken = default);
    Task SetCommandStatusAsync(Guid applicationId, Guid commandId, string setBy, CommandStatus commandStatus, string message, CancellationToken cancellationToken = default); 
    Task<CommandQueueItem> QueueCommandAsync(Guid applicaitonId, string command,  string addedBy, CancellationToken cancellationToken = default);
}
