namespace ApplicationControl.CommandProcessor;

public interface ICommandParser
{
    Task<Command> ParseAsync(string command);
}
