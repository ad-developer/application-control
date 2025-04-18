namespace ApplicationControl.Core.Common;

public enum CommandStatus
{
    Queued = 1,
    InProcess = 2,
    Completed = 3,
    Failed = 4,
    Undefined = 0
}
