using ApplicationControl.Core.Common;
using Microsoft.EntityFrameworkCore;

namespace ApplicationControl.Core;

public class ApplicationControlRepository(IApplicationControlContext context) : BaseRepository<ApplicationControl, Guid>(context), IApplicationControlRepository
{
    public async Task<ApplicationControl?> GetNextCommandAsync(CancellationToken cancellationToken = default)
    {
          var nextCommand = 
              await  Entity
                        .Where(p => p.Status == CommandStatus.Queued)
                        .OrderBy(p => p.AddedDateTime)
                        .SingleOrDefaultAsync(cancellationToken);
        
        return  nextCommand;
    }

    public async Task SetCommandStatusAsync(Guid applicaitonId, Guid commandId, string setBy, CommandStatus commandStatus, string message, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(setBy, nameof(setBy));
        
        var cmd = await Entity
                .Where(p=>p.ApplicationId == applicaitonId && p.Id == commandId)
                .SingleOrDefaultAsync(cancellationToken);
            
        if (cmd is null)
        {
            throw new InvalidOperationException($"Command with ID {commandId} and applicationId {applicaitonId} not found.");
        }
        
        cmd.Status = commandStatus;
        if(!string.IsNullOrEmpty(message))
        {
            cmd.Message = message;
        }
        await UpdateAsync(cmd,setBy, cancellationToken);
    }
}