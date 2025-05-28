using ApplicationControl.Core.Extensions;

namespace ApplicationControl.Client.Shared;

public interface IJobsRunner
{
    Task RunJobsAsync(List<IJob> jobs, CancellationToken cancellationToken = default);
}
