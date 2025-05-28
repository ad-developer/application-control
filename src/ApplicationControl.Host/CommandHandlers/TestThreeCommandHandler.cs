using ApplicationControl.CommandProcessor;

namespace ApplicationControl.Host.CommandHandlers;

[Command("testthree")]
public class TestThreeCommandHandler : ICommandHandler
{
    public IServiceProvider Services { get; }

    public TestThreeCommandHandler(IServiceProvider services)
    {
        Services = services;
    }
    public async Task HandleAsync(Command command)
    {
        Task.Delay(5000).Wait();
        Console.WriteLine($"TestThreeCommandHandler command {command.Name} executed ");

        Console.WriteLine(string.Empty);

        throw new Exception("TestThreeCommandHandler exception");
    }
}