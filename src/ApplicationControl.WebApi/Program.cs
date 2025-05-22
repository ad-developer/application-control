using ApplicationControl.Core;
using ApplicationControl.Core.Configuration;
using ApplicationControl.Core.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureApplicationControlCoreService(options =>
{
    options.ConnectionString = "ApplicationControlDb";
});
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Services
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
        {
            options.WithTitle("Application Control WebAPI")
                .WithTheme(ScalarTheme.Mars)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                
        });
}

app.UseHttpsRedirection();

app.MapGet("/getnextqueuedjob", async (Guid applicationId, IApplicationControlService service) =>
{ 
    var command = await service.GetQueuedJobAsync(applicationId);    
    return TypedResults.Ok(command);
})
.WithName("GetNextQueuedJob");

app.MapPost("/setquuedjobstatus", async (Guid applicaiotnId, Guid commandId, string setBy, JobStatus jobStatus, string message, IApplicationControlService service) =>
{
    await service.SetQueuedJobStatusAsync(applicaiotnId, commandId, setBy, jobStatus, message);
})
.WithName("SetQueuedJobStatus");

app.MapPost("/queuequeuedjob", async (Guid applicaitonId, string command, string addedBy, IApplicationControlService service) =>
{
    var res = await service.QueueQueuedJobAsync(applicaitonId, command, addedBy);
    return TypedResults.Ok(res);
})
.WithName("QueueQueuedJob");

app.Run();