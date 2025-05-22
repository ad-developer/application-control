using ApplicationControl.Core.Entities;
using ApplicationControl.Core.Extensions;


namespace ApplicationControl.Core.Respositories;

public class ApplicationJobRepository(IApplicationControlContext context) : BaseRepository<ApplicationJob, Guid>(context), IApplicationJobRepository
{
}