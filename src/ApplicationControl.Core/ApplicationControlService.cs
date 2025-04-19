using ApplicationControl.Core.Common;

namespace ApplicationControl.Core;

public class ApplicationControlService(ICommandQueueItemRepository commandQueueItemRepository) : IApplicationControlService
{
    private readonly ICommandQueueItemRepository _commandQueueItemRepository = commandQueueItemRepository;

    public async Task<CommandQueueItem> QueueCommandAsync(Guid applicaitonId, string command,  string addedBy, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(applicaitonId.ToString(), nameof(applicaitonId));
        ArgumentException.ThrowIfNullOrEmpty(command, nameof(command));
        ArgumentException.ThrowIfNullOrEmpty(addedBy, nameof(addedBy));
        
        var commandEntity = new CommandQueueItem
        {
            ApplicationId = applicaitonId,
            Command = command,
            Status = CommandStatus.Queued
        };
        
        var res = await _commandQueueItemRepository.AddAsync(commandEntity, addedBy, cancellationToken);
        return res;
    }

    public async Task<CommandQueueItem?> GetNextCommandAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(applicationId.ToString(), nameof(applicationId));

        var nextCommand = await _commandQueueItemRepository.GetNextCommandAsync(cancellationToken);

        return nextCommand;
    }

    public async Task SetCommandStatusAsync(Guid applicationId, Guid commandId, string setBy, CommandStatus commandStatus, string message, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(applicationId.ToString(), nameof(applicationId));
        ArgumentException.ThrowIfNullOrEmpty(commandId.ToString(), nameof(commandId));
        ArgumentException.ThrowIfNullOrEmpty(setBy, nameof(setBy));
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));
        ArgumentException.ThrowIfNullOrEmpty(commandStatus.ToString(), nameof(commandStatus));

        await _commandQueueItemRepository.SetCommandStatusAsync(applicationId, commandId, setBy, commandStatus, message, cancellationToken);
    }
}