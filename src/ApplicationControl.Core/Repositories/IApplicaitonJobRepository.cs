using ApplicationControl.Core.Entities;
using ApplicationControl.Core.Extensions;

namespace ApplicationControl.Core.Respositories;

public interface IApplicationJobRepository : IBaseRepository<ApplicationJob, Guid>
{
}