namespace ApplicationControl.CommandProcessor;

public class CommandProcessor : ICommandProcessor
{
    private readonly ICommandParser _commandParser;
    private  readonly IServiceProvider _services;
    public CommandProcessor(ICommandParser commandParser, IServiceProvider services)
    {
        _commandParser =commandParser;
        _services = services;
    }
    
    public async Task PprocessAsync(string command)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command);
        
        var cmd = await _commandParser.ParseAsync(command);
        var handler = InvokeCommandHandler(cmd.Name);
        
        await handler.HandleAsync(cmd);
    }

    internal ICommandHandler InvokeCommandHandler(string command)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command, nameof(command));
        Type? handlerType;
        if(!CommandDictionary.Items.TryGetValue(command, out handlerType))
        {
            throw new Exception($"There is no command handler for {command} command.");
        }
        
        var handlerInstance = Activator.CreateInstance(handlerType, _services) as ICommandHandler 
            ?? throw new Exception("Failed to create instance of ICommandHandler");
        
        return handlerInstance;
    }
}
