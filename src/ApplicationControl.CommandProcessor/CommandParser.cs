namespace ApplicationControl.CommandProcessor;

public class CommandParser : ICommandParser
{
    public async Task<Command> ParseAsync(string command)
    {
        ArgumentException.ThrowIfNullOrEmpty(command, nameof(command));
        var parts = command.Split('-');
        Command res;
        
        if(parts.Length > 1){
            var name = parts[0].Trim();
            res = await GenerateCommandAsync(name);
        }
        else 
        {
            res = await GenerateCommandWithOptionsAsync(parts);
        }
        
        return await Task.FromResult(res);
    }

    internal Task<Command> GenerateCommandAsync(string commandName){
        // Split by white-space character
        var possibleParts = commandName.Split(null);
        if(possibleParts.Length > 1)
            throw new Exception("Command must be a single wods");
        
        var cmd = new Command{
             Name =commandName,
            
        };          
        return Task.FromResult(cmd);    
    }

    internal async Task<Command> GenerateCommandWithOptionsAsync(string[] commandWithOptions){
       
        var name = commandWithOptions[0].Trim();
        var command = await GenerateCommandAsync(name);
        command.Options = new Dictionary<string, string>();
        
        for (int i = 1; i < commandWithOptions.Length; i++)
        {
            var part = commandWithOptions[i];
            
            // Split by white-space character
            var pair =  part.Split(null); 
            
            var option = pair[0].Trim();
            var optionValue = string.Empty;
            
            if(pair.Length > 2)
                throw new Exception($"The option {option} value must be a single word."); 

            if(pair.Length > 1)
                optionValue = pair[1].Trim();

            command.Options.Add(option, optionValue);
        }

        return command;
    }
}
