using ApplicationControl.Core.Common;
using ApplicationControl.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationControl.Core.Respositories;

public class QueuedApplicationJobRepository(IApplicationControlContext context) : BaseRepository<QueuedApplicationJob, Guid>(context), IQueuedApplicationJobRepository
{
    public async Task<QueuedApplicationJob?> GetNextJobAsync(CancellationToken cancellationToken = default)
    {
          var nextCommand = 
              await  Entity
                        .Where(p => p.Status == QueuedJobStatus.Queued)
                        .OrderBy(p => p.AddedDateTime)
                        .SingleOrDefaultAsync(cancellationToken);
        
        return  nextCommand;
    }

    public async Task SetJobStatusAsync(Guid applicaitonId, Guid commandId, string setBy, QueuedJobStatus queuedJobStatus, string message, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(setBy, nameof(setBy));
        
        var cmd = await Entity
                .Where(p=>p.ApplicationId == applicaitonId && p.Id == commandId)
                .SingleOrDefaultAsync(cancellationToken);
            
        if (cmd is null)
        {
            throw new InvalidOperationException($"Command with ID {commandId} and applicationId {applicaitonId} not found.");
        }
        
        cmd.Status = queuedJobStatus;
        if(!string.IsNullOrEmpty(message))
        {
            cmd.Message = message;
        }
        await UpdateAsync(cmd,setBy, cancellationToken);
    }
}