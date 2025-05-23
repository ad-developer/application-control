using ApplicationControl.Client.Database.Entities;
using ApplicationControl.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ApplicationControl.Client.Database.Repositories;

public class QueuedApplicationJobRepository(IApplicationControlContext context) : BaseRepository<QueuedApplicationJob, Guid>(context), IQueuedApplicationJobRepository
{
    public async Task<List<IJob>> GetNextJobListAsync(CancellationToken cancellationToken)
    {
        var commandList = 
              await  Entity
                        .Where(p => p.Status == JobStatus.Queued)
                        .OrderBy(p => p.AddedDateTime)
                        .ToListAsync<IJob>(cancellationToken);
        
        return commandList;
    }
    public async Task SetJobStatusAsync(Guid applicaitonId, Guid jobId, string setBy, JobStatus queuedJobStatus, string? message, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(setBy, nameof(setBy));
        
        var cmd = await Entity
                .Where(p=>p.ApplicationId == applicaitonId && p.Id == jobId)
                .SingleOrDefaultAsync(cancellationToken);
            
        if (cmd is null)
            throw new Exception($"Job Id {jobId} with applicationId {applicaitonId} not found.");
        
        
        cmd.Status = queuedJobStatus;
        if(!string.IsNullOrEmpty(message))
        {
            cmd.Message = message;
        }
        await UpdateAsync(cmd,setBy, cancellationToken);
    }
}