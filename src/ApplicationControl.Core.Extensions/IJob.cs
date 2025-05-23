namespace ApplicationControl.Core.Extensions;

public interface IJob
{
    public Guid Id { get; set; }
    public string Command { get; set; }
    public JobStatus Status { get; set; }
}
