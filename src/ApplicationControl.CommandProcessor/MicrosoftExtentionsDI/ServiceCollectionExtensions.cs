using Microsoft.Extensions.DependencyInjection;

namespace ApplicationControl.CommandProcessor.MicrosoftExtentionsDI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandProcessor(this IServiceCollection services, 
        Action<CommandProcessorServiceConfiguration> configuration)
    {
        var serviceConfig = new CommandProcessorServiceConfiguration();

        configuration.Invoke(serviceConfig);

        return services.AddCommandProcessor(serviceConfig);
    }
    
    public static IServiceCollection AddCommandProcessor(this IServiceCollection services, 
        CommandProcessorServiceConfiguration configuration)
    {   
        if (!configuration.AssembliesToRegister.Any())
        {
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");
        }
        
        services.AddScoped<ICommandProcessor, CommandProcessor>();
        services.AddScoped<ICommandParser, CommandParser>();
        
        configuration.RegisterHandlersFromAssembly();
        
        return services;
    }
}
