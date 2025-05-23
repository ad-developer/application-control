namespace ApplicationControl.Client.Configuration;

public class Settings
{
    public string? ConnectionString { get; set; }

    public string? ApplicationName { get; set; }
    public Guid ApplicaitonId { get; set; }

    // The time in seconds to wait before checking for new jobs
    public int QueuedHastedServiceResetProcessCycle { get; set; } = 60;
    public int QueuedHastedServiceCycle { get; set; } = 60; 
    public int QueuedHastedResetServiceCycle { get; set; } = 120; 
}
