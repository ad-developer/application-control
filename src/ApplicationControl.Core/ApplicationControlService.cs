using ApplicationControl.Core.Common;

namespace ApplicationControl.Core;

public class ApplicationControlService(IApplicationControlRepository applicationControlRepository) : IApplicationControlService
{
    private readonly IApplicationControlRepository _applicationControlRepository = applicationControlRepository;

    public async Task<ApplicationControl> QueueCommandAsync(Guid applicaitonId, string command,  string addedBy, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(applicaitonId.ToString(), nameof(applicaitonId));
        ArgumentException.ThrowIfNullOrEmpty(command, nameof(command));
        ArgumentException.ThrowIfNullOrEmpty(addedBy, nameof(addedBy));
        
        var commandEntity = new ApplicationControl
        {
            ApplicationId = applicaitonId,
            Command = command,
            Status = CommandStatus.Queued
        };
        
        var res = await _applicationControlRepository.AddAsync(commandEntity, addedBy, cancellationToken);
        return res;
    }

    public async Task<ApplicationControl?> GetNextCommandAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(applicationId.ToString(), nameof(applicationId));

        var nextCommand = await _applicationControlRepository.GetNextCommandAsync(cancellationToken);

        return nextCommand;
    }

    public async Task SetCommandStatusAsync(Guid applicationId, Guid commandId, string setBy, CommandStatus commandStatus, string message, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(applicationId.ToString(), nameof(applicationId));
        ArgumentException.ThrowIfNullOrEmpty(commandId.ToString(), nameof(commandId));
        ArgumentException.ThrowIfNullOrEmpty(setBy, nameof(setBy));
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));
        ArgumentException.ThrowIfNullOrEmpty(commandStatus.ToString(), nameof(commandStatus));

        await _applicationControlRepository.SetCommandStatusAsync(applicationId, commandId, setBy, commandStatus, message, cancellationToken);
    }
    
}