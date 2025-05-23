namespace ApplicationControl.Client.Database.HostedServices;

public interface IDatabaseQueuedJobWorker
{
    Task DoWorkAsync(CancellationToken cancellationToken);
}
