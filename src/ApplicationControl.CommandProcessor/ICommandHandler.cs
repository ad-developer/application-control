namespace ApplicationControl.CommandProcessor;

public interface ICommandHandler
{
    IServiceProvider Services { get; }
    Task HandleAsync(Command command);
}
