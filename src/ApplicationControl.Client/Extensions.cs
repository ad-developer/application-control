using ApplicationControl.Client.Configuration;
using ApplicationControl.Client.Database;
using ApplicationControl.Client.Database.hostedServices;
using ApplicationControl.Client.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ApplicationControl.Client.Database.Repositories;


namespace ApplicationControl.Client;

public static class Extensions
{
    public static IHostBuilder AddDatabaseApplicationControl(this IHostBuilder hostBuilder)
    {

        hostBuilder.ConfigureServices((context, services) =>
        {
            services.Configure<ApplicationControlOptions>(context.Configuration.GetSection("ApplicationControl"));

            var config = context.Configuration.GetSection("ApplicationControl").Get<ApplicationControlOptions>();
            
            services.AddDbContext<ApplicationControlContext>(options =>
            {
                if (config is not null)
                {
                    var connectionString = config.ConnectionString;
                    options.UseSqlServer(connectionString);    
                }
            });
            
            services.AddScoped<IQueuedApplicationJobRepository, QueuedApplicationJobRepository>();
            services.AddScoped<IApplicationControlContext, ApplicationControlContext>();
            services.AddScoped<IJobRunner, JobRunner>();
            services.AddHostedService<DatabaseQueuedJobHostedService>();         
        });

        return hostBuilder;   
    }
}
