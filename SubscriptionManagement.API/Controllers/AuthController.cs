namespace SubscriptionManagement.API.Controllers;

public class AuthController : BaseController
{
	private readonly IAuthService _authService;

	public AuthController(IAuthService authService)
	{
		_authService = authService;
	}

	/// <summary>
	/// Login into our system through this API
	/// </summary>
	/// <param name="dto">
	/// Email : your email address that already registered on our system,
	/// Password: your valid password
	/// </param>
	/// <returns>the jwt token with the user information</returns>
	[HttpPost]
	public async Task<IActionResult> Login(LoginDto dto)
	{
		var response = await _authService.Login(dto);
		return StatusCode((int)response.Code, response);
	}

	/// <summary>
	/// Register your account on our system through this API
	/// </summary>
	/// <param name="dto">
	/// FirstName : is required,
	/// LastName: is required,
	/// Password: is required and must be at least 8 characters,
	/// ConirmPassword: should be same as above password
	/// </param>
	/// <returns>the jwt token with the user information</returns>
	[HttpPost]
	public async Task<IActionResult> Register(RegisterDto dto)
	{
		var response = await _authService.Register(dto);
		return StatusCode((int)response.Code, response);
	}
}