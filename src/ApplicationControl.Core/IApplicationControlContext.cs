using ApplicationControl.Core.Entities;
using ApplicationControl.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ApplicationControl.Core;

public interface IApplicationControlContext : IContext
{
     DbSet<QueuedApplicationJob> QueuedApplicationJobs { get; set; }
}
