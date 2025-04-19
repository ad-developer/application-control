using Microsoft.EntityFrameworkCore;

namespace ApplicationControl.Core;

public class ApplicationControlContext : DbContext, IApplicationControlContext
{
    public virtual  DbSet<CommandQueueItem> CommandQueueItems { get; set; }

    public ApplicationControlContext(DbContextOptions<ApplicationControlContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommandQueueItem>().HasKey(x => x.Id);
    }
}
