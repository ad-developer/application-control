using ApplicationControl.CommandProcessor;

namespace ApplicationControl.Host.CommandHandlers;

[Command("testone")]
public class TestOneCommandHandler : ICommandHandler
{
    public IServiceProvider Services { get; }

    public TestOneCommandHandler(IServiceProvider services)
    {
        Services = services;
    } 

    public async Task HandleAsync(Command command)
    {
        Task.Delay(1000).Wait();
        Console.WriteLine($"TestOneCommandHandler command {command.Name} excecuted ");

        Console.WriteLine(string.Empty);
    }
}
