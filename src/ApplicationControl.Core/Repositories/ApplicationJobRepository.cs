using ApplicationControl.Core.Common;
using ApplicationControl.Core.Entities;


namespace ApplicationControl.Core.Respositories;

public class ApplicationJobRepository(IApplicationControlContext context) : BaseRepository<ApplicationJob, Guid>(context), IApplicationJobRepository
{

}