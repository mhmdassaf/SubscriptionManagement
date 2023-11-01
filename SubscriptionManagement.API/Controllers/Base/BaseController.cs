namespace SubscriptionManagement.API.Controllers.Base;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public abstract class BaseController : ControllerBase
{
}
