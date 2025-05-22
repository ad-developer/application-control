using System.Reflection;

namespace ApplicationControl.CommandProcessor.Extensions;

public class CommandProcessorServiceConfiguration
{
    internal List<Assembly> AssembliesToRegister { get; } = new();
   
    public CommandProcessorServiceConfiguration RegisterServicesFromAssemblyContaining<T>()
        => RegisterServicesFromAssemblyContaining(typeof(T));

    public CommandProcessorServiceConfiguration RegisterServicesFromAssemblyContaining(Type type)
        => RegisterServicesFromAssembly(type.Assembly);
    
    public CommandProcessorServiceConfiguration RegisterServicesFromAssembly(Assembly assembly)
    {
        AssembliesToRegister.Add(assembly);

        return this;
    }

    public CommandProcessorServiceConfiguration RegisterHandlersFromAssembly()
    {
        var commanHandlerType = typeof(ICommandHandler);
        var types = AssembliesToRegister
                .SelectMany(a => a.DefinedTypes)
                .Where(p => commanHandlerType.IsAssignableFrom(p))
                .ToList();
        
        types.ForEach(type =>{
            var attrs = Attribute.GetCustomAttributes(type); 
            foreach (Attribute attr in attrs)
            {
                if (attr is CommandAttribute a)
                {
                    CommandDictionary.Items.Add(a.Name, type);
                }
            }
        });

        return this;
    }
}
