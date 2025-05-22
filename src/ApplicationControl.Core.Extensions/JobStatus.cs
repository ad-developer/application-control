using System.Text.Json.Serialization;

namespace ApplicationControl.Core.Extensions;

[JsonConverter(typeof(JsonStringEnumConverter<JobStatus>))]
public enum JobStatus
{
    Queued = 1,
    InProgress = 2,
    Completed = 3,
    Failed = 4,
    Undefined = 0
}