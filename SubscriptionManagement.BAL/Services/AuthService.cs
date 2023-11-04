using System.Net;

namespace SubscriptionManagement.BAL.Services;

public class AuthService : BaseService, IAuthService
{
	private readonly AuthSettings _authSettings;
	private readonly UserManager<User> _userManager;

	public AuthService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
		IOptions<AuthSettings> options,
		UserManager<User> userManager,
		ILogger<AuthService> logger) : base(repository, mapper, httpContextAccessor, logger)
	{
		_authSettings = options.Value;
		_userManager = userManager;
	}

	public async Task<ResponseModel> Login(LoginDto dto)
	{
		// Authenticate user (e.g., validate credentials)
		var user = await _userManager.FindByNameAsync(dto.Email);
		if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
		{
			// User is authenticated, generate JWT token
			var token = GenerateJwtToken(user);

			ResponseModel.Code = HttpStatusCode.OK;
			ResponseModel.Message = Validation.Auth.LoginSuccessMsg;
			_logger.LogInformation($"{nameof(Login)} {Validation.Auth.LoginSuccessMsg}");
			ResponseModel.Result = new LoginModel
			{
				UserName = user.UserName,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Token = token
			};
	    }
		else
		{
			ResponseModel.Code = HttpStatusCode.BadRequest;
			ResponseModel.Errors.Add(new ErrorModel
			{
				Code = Validation.Auth.UserNotExistOrWrongPasswordCode,
				Message = Validation.Auth.UserNotExistOrWrongPasswordMsg
			});
			_logger.LogWarning($"{nameof(Login)} {Validation.Auth.UserNotExistOrWrongPasswordCode}");
		}
		return ResponseModel;
	}

	public async Task<ResponseModel> Register(RegisterDto dto)
	{
		var user = _mapper.Map<User>(dto);
		var result = await _userManager.CreateAsync(user, dto.Password);

		if (result == null)
		{
			ResponseModel.Code = HttpStatusCode.BadRequest;
			ResponseModel.Errors.Add(new ErrorModel
			{
				Code = Validation.Auth.RegisterFailCode,
				Message = Validation.Auth.RegisterFailMsg
			});
			_logger.LogWarning($"{nameof(Register)} {Validation.Auth.RegisterFailCode}");
			return ResponseModel;
		}

		if (result.Succeeded)
		{
			ResponseModel.Message = Validation.Auth.RegisterSuccessMsg;
			ResponseModel.Code = HttpStatusCode.Created;
			_logger.LogInformation($"{nameof(Register)} {Validation.Auth.RegisterSuccessMsg}");
		}
		else
		{
			ResponseModel.Code = HttpStatusCode.InternalServerError;
			ResponseModel.Errors.AddRange(result.Errors.Select(s => new ErrorModel
			{
				Code = s.Code,
				Message = s.Description
			}));
			_logger.LogWarning($"{nameof(Register)} {HttpStatusCode.InternalServerError}");
		}

		return ResponseModel;
	}

	#region Private
	private string GenerateJwtToken(User user)
	{
		var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.SecretKey));
		var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
		var authClaims = new List<Claim>()
		{
			new Claim(ClaimTypes.Name, user.UserName),
		     new Claim(ClaimTypes.Email, user.Email)
		};

		var tokeOptions = new JwtSecurityToken(
			issuer: _authSettings.Issuer, 
			audience: _authSettings.Audience, 
			claims: authClaims, 
			expires: DateTime.Now.AddSeconds(_authSettings.Expires), 
			signingCredentials: signinCredentials);

		var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
		return tokenString;
	}

	#endregion
}

public interface IAuthService
{
	Task<ResponseModel> Login(LoginDto dto);
	Task<ResponseModel> Register(RegisterDto dto);
}