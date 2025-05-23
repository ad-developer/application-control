using ApplicationControl.Client.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationControl.Client.Database;

public class ApplicationControlContext : DbContext, IApplicationControlContext
{
    public virtual  DbSet<QueuedApplicationJob> QueuedApplicationJobs { get; set; }

    public ApplicationControlContext(DbContextOptions<ApplicationControlContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QueuedApplicationJob>().HasKey(x => x.Id);
    }
}
