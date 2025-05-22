using ApplicationControl.Core.Respositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApplicationControl.Core.Configuration;

public static class ApplicationControlExtentions
{
    public static void ConfigureApplicationControlCoreService(this IHostApplicationBuilder builder, Action<ApplicationControlOptions> options){
        var services = builder.Services;
           
        //services.Configure<ApplicationControlOptions>(builder.Configuration.GetSection("ApplicationControlOptionsCore"));
        
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        
        ApplicationControlOptions op = new();
        options(op);
        ArgumentException.ThrowIfNullOrEmpty(op.ConnectionString, op.ConnectionString);

        services.AddDbContext<ApplicationControlContext>(options=>{
            var connectionString =
                builder.Configuration.GetConnectionString(op.ConnectionString);
            options.UseSqlServer(connectionString);
        });
        services.AddScoped<IApplicationControlContext, ApplicationControlContext>();
        services.AddScoped<IQueuedApplicationJobRepository, QueuedApplicationJobRepository>();
        services.AddScoped<IApplicationControlService,ApplicationControlService>();
    }
}