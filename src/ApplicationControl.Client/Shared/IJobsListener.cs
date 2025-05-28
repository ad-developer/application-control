namespace ApplicationControl.Client.Shared;

public interface IJobsListener
{
    Task ListenAsync(CancellationToken cancellationToken = default);
}
