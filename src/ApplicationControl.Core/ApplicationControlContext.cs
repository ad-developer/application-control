using ApplicationControl.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationControl.Core;

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
