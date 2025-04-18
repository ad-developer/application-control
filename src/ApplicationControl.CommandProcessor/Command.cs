namespace ApplicationControl.CommandProcessor;

public class Command
{
    public required string Name { get; set; }
    public Dictionary<string, string> Options { get; set; } = [];
}