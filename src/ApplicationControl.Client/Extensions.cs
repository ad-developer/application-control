using ApplicationControl.Client.Configuration;
using ApplicationControl.Client.Database;
using ApplicationControl.Client.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ApplicationControl.Client.Database.Repositories;



namespace ApplicationControl.Client;

public static class Extensions
{
    public static IHostBuilder AddQueduedJobsApplicationControl(this IHostBuilder hostBuilder)
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
            }, ServiceLifetime.Transient);

            if (config is not null && config.ApplicationClientType == ClinetType.Database)
            {
                services.AddScoped<IQueuedApplicationJobRepository, QueuedApplicationJobRepository>();
                services.AddTransient<IApplicationControlContext, ApplicationControlContext>();
                services.AddTransient<IJobsSource, DatabaseQueuedJobsSource>();
            }

            services.AddScoped<IJobsListener, JobsListener>();
            services.AddScoped<IJobsRunner, JobsRunner>();
            services.AddScoped<IJobWorker, JobWorker>();
            services.AddHostedService<QueuedJobsHostedService>();
                     
        });

        return hostBuilder;   
    }
}
