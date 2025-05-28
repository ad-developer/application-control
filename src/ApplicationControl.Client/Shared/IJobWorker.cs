using ApplicationControl.Core.Extensions;

namespace ApplicationControl.Client.Shared;

public interface IJobWorker
{
    Task RunJobAsync(IJob job, CancellationToken cancellationToken = default);
}
