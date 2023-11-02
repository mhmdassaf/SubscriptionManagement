namespace SubscriptionManagement.API.Controllers;

public class AuthController : BaseController
{
	private readonly IAuthService _authService;

	public AuthController(IAuthService authService)
	{
		_authService = authService;
	}

	[HttpPost]
	public async Task<IActionResult> Login(LoginDto dto)
	{
		return Ok(await _authService.Login(dto));
	}

	[HttpPost]
	public async Task<IActionResult> Register(RegisterDto dto)
	{
		return Ok(await _authService.Register(dto));
	}
}