using ApplicationControl.Core.Common;

namespace ApplicationControl.Core;

public interface ICommandQueueItemRepository : IBaseRepository<CommandQueueItem, Guid>
{
    Task<CommandQueueItem?> GetNextCommandAsync(CancellationToken cancellationToken);
    Task SetCommandStatusAsync(Guid applicationId, Guid commandId, string setBy,CommandStatus commandStatus, string message, CancellationToken cancellationToken);
}
