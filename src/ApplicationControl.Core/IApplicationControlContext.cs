using ApplicationControl.Core.Common;
using Microsoft.EntityFrameworkCore;

namespace ApplicationControl.Core;

public interface IApplicationControlContext : IContext
{
     DbSet<CommandQueueItem> CommandQueueItems { get; set; }
}
