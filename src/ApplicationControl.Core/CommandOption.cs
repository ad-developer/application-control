namespace ApplicationControl.Core;

public record  CommandOption
{
    public required string Optioion { get; set; }
    public List<string> Values  { get; set; } = [];
}
