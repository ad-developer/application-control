using ApplicationControl.Core.Common;

namespace ApplicationControl.Core;

public interface IApplicationControlRepository : IBaseRepository<ApplicationControl, Guid>
{
    Task<ApplicationControl?> GetNextCommandAsync(CancellationToken cancellationToken);
    Task SetCommandStatusAsync(Guid applicationId, Guid commandId, string setBy,CommandStatus commandStatus, string message, CancellationToken cancellationToken);
}
