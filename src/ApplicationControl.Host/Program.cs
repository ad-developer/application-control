using ApplicationControl.Client;
using ApplicationControl.CommandProcessor.Extensions;
using ApplicationControl.Host.CommandHandlers;

var host = Host.CreateDefaultBuilder(args);
host.ConfigureServices((context, services) =>
{
     services.AddCommandProcessor(p=>{
      p.RegisterServicesFromAssembly(typeof(TestOneCommandHandler).Assembly);
   });
});

host.AddQueduedJobsApplicationControl();

var app = host.Build();
await app.StartAsync();

await app.WaitForShutdownAsync();