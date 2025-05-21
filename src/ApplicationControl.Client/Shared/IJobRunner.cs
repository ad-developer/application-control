namespace ApplicationControl.Client.Shared;

public interface IJobRunner
{
    Task RunJobAsync(List<IJob>jobs, Action<IJob> action,  CancellationToken cancellationToken);
}
