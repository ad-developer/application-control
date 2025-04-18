namespace ApplicationControl.CommandProcessor;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CommandAttribute : Attribute
{
    public string Name;
    public CommandAttribute(string name)
    {
        Name = name;
    }
}