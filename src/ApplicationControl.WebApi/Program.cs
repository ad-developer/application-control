using ApplicationControl.Core;
using ApplicationControl.Core.Configuration;
using ApplicationControl.Core.Common;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureApplicationControlCoreService(options =>
{
    options.ConnectionName = "ApplicationControlDb";
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

app.MapGet("/getnextcommand", async (Guid applicationId, IApplicationControlService service) =>
{ 
    var command = await service.GetNextCommandAsync(applicationId);    
    return Results.Ok(command);
})
.WithName("GetNextCommand");

app.MapPost("/setstatus", async (Guid applicaiotnId, Guid commandId, string setBy, CommandStatus status, string message, IApplicationControlService service) =>
{
    await service.SetCommandStatusAsync(applicaiotnId, commandId, setBy, status, message);
})
.WithName("SetStatus");

app.MapPost("/queuecommand", async (Guid applicaitonId, string command, string addedBy, IApplicationControlService service) =>
{
    var res = await service.QueueCommandAsync(applicaitonId, command, addedBy);
    return Results.Ok(res);
})
.WithName("QueueCommand");

app.Run();