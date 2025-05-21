using ApplicationControl.Core.Common;
using ApplicationControl.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationControl.Core;

public interface IApplicationControlContext : IContext
{
     DbSet<QueuedApplicationJob> QueuedApplicationJobs { get; set; }
}
