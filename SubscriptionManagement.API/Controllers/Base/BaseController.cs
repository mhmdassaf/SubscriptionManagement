namespace SubscriptionManagement.API.Controllers.Base;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public abstract class BaseController : ControllerBase
{
}
