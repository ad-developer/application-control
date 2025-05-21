using System.Text.Json.Serialization;

namespace ApplicationControl.Core.Common;

[JsonConverter(typeof(JsonStringEnumConverter<QueuedJobStatus>))]
public enum QueuedJobStatus
{
    Queued = 1,
    InProcess = 2,
    Completed = 3,
    Failed = 4,
    Undefined = 0
}