using ApplicationControl.Core.Extensions;

namespace ApplicationControl.Client.Shared;

public interface IJobRunner
{
    Task RunJobAsync(List<IJob>jobs,   CancellationToken cancellationToken);
}
