using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Dapper;
using Cronos;
using System.Collections.Generic;
using System.Linq;

public class ScheduledJobsHostedService : BackgroundService
{
    private readonly ILogger<ScheduledJobsHostedService> _logger;
    private readonly string _connectionString;

    public ScheduledJobsHostedService(ILogger<ScheduledJobsHostedService> logger)
    {
        _logger = logger;
        _connectionString = "Server=.;Database=MyAppDb;Trusted_Connection=True;";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Job Scheduler Service with CRON + catch-up started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckAndRunJobsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while checking or running jobs.");
            }

            // Check every 30 seconds
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private async Task CheckAndRunJobsAsync()
    {
        var now = DateTime.UtcNow;

        using var connection = new SqlConnection(_connectionString);
        var jobs = await connection.QueryAsync<ScheduledJob>("SELECT * FROM ScheduledJobs");

        foreach (var job in jobs)
        {
            if (!ShouldRun(job, now)) continue;

            _logger.LogInformation($"[Job {job.Id}] Executing '{job.JobName}' (scheduled: {job.ScheduledTime:u})");

            try
            {
                await RunJobAsync(job);

                var nextRun = CalculateNextRun(job, now);
                if (nextRun.HasValue)
                {
                    await connection.ExecuteAsync(
                        "UPDATE ScheduledJobs SET ScheduledTime = @NextRun, LastRunTime = @Now WHERE Id = @Id",
                        new { Id = job.Id, Now = now, NextRun = nextRun.Value }
                    );
                    _logger.LogInformation($"[Job {job.Id}] Rescheduled for {nextRun:u}");
                }
                else
                {
                    await connection.ExecuteAsync(
                        "UPDATE ScheduledJobs SET IsExecuted = 1, LastRunTime = @Now WHERE Id = @Id",
                        new { Id = job.Id, Now = now }
                    );
                    _logger.LogInformation($"[Job {job.Id}] Marked as executed (one-time).");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Job {job.Id}] Failed to execute job '{job.JobName}'.");
            }
        }
    }

    private bool ShouldRun(ScheduledJob job, DateTime now)
    {
        // Never run already executed one-time jobs
        if (!job.IsRecurring && job.IsExecuted)
            return false;

        // Normal case: scheduled time reached or passed
        if (job.ScheduledTime <= now)
            return true;

        // If missed and flagged for catch-up
        if (job.RunIfMissed && job.LastRunTime.HasValue)
        {
            // If we missed a CRON interval (e.g. app was offline)
            if (job.CronExpression != null)
            {
                var cron = CronExpression.Parse(job.CronExpression, CronFormat.Standard);
                var nextExpected = cron.GetNextOccurrence(job.LastRunTime.Value, TimeZoneInfo.Utc);
                return nextExpected.HasValue && nextExpected.Value <= now;
            }
        }

        return false;
    }

    private DateTime? CalculateNextRun(ScheduledJob job, DateTime now)
    {
        if (!job.IsRecurring)
            return null;

        if (!string.IsNullOrEmpty(job.CronExpression))
        {
            var cron = CronExpression.Parse(job.CronExpression, CronFormat.Standard);
            return cron.GetNextOccurrence(now, TimeZoneInfo.Utc);
        }

        if (job.IntervalMinutes.HasValue)
        {
            return now.AddMinutes(job.IntervalMinutes.Value);
        }

        return null;
    }

    private Task RunJobAsync(ScheduledJob job)
    {
        switch (job.Command?.ToLowerInvariant())
        {
            case "runreport":
                _logger.LogInformation($"Running report for job {job.Id}...");
                break;
            case "cleanup":
                _logger.LogInformation($"Performing cleanup for job {job.Id}...");
                break;
            case "sendemail":
                _logger.LogInformation($"Sending email for job {job.Id}...");
                break;
            default:
                _logger.LogWarning($"Unknown command '{job.Command}' for job {job.Id}");
                break;
        }

        // Simulate real work
        return Task.Delay(1000);
    }
}

public class ScheduledJob
{
    public int Id { get; set; }
    public string JobName { get; set; } = "";
    public DateTime ScheduledTime { get; set; }
    public bool IsExecuted { get; set; }
    public bool IsRecurring { get; set; }
    public int? IntervalMinutes { get; set; }
    public string? CronExpression { get; set; }
    public string? Command { get; set; }
    public DateTime? LastRunTime { get; set; }
    public bool RunIfMissed { get; set; } = true;
}
