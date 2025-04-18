namespace ApplicationControl.CommandProcessor;

public interface ICommandProcessor
{
    Task PprocessAsync(string command);
}
