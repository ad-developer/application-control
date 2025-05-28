using ApplicationControl.CommandProcessor;

namespace ApplicationControl.Host.CommandHandlers;

[Command("testtwo")]
public class TestTwoCommandHandler : ICommandHandler
{
    public IServiceProvider Services { get; }

    public TestTwoCommandHandler(IServiceProvider services)
    {
        Services = services;
    }
    public async Task HandleAsync(Command command)
    {
        Task.Delay(10000).Wait();
        Console.WriteLine($"TestTwoCommandHandler command {command.Name} excecuted ");

        Console.WriteLine(string.Empty);
    }
}