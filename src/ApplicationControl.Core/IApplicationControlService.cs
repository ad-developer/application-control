using ApplicationControl.Core.Common;


namespace ApplicationControl.Core;

public interface IApplicationControlService
{
    Task<ApplicationControl?> GetNextCommandAsync(Guid applicationId, CancellationToken cancellationToken);
    Task SetCommandStatusAsync(Guid applicationId, Guid commandId, string setBy, CommandStatus commandStatus, string message, CancellationToken cancellationToken);
    Task<ApplicationControl> QueueCommandAsync(Guid applicaitonId, string command,  string addedBy, CancellationToken cancellationToken);
}
