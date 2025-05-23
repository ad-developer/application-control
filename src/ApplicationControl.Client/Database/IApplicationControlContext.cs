using ApplicationControl.Client.Database.Entities;
using ApplicationControl.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ApplicationControl.Client.Database;

public interface IApplicationControlContext : IContext
{
     DbSet<QueuedApplicationJob> QueuedApplicationJobs { get; set; }
}
