using System.Net;

namespace ManageProducts.BAL.Services;

public class AuthService : BaseService, IAuthService
{
	private readonly AuthSettings _authSettings;
	private readonly UserManager<User> _userManager;

	public AuthService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
		IOptions<AuthSettings> options,
		UserManager<User> userManager) : base(repository, mapper, httpContextAccessor)
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
			return ResponseModel;
		}

		if (result.Succeeded)
		{
			ResponseModel.Message = Validation.Auth.RegisterSuccessMsg;
			ResponseModel.Code = HttpStatusCode.Created;
		}
		else
		{
			ResponseModel.Code = HttpStatusCode.InternalServerError;
			ResponseModel.Errors.AddRange(result.Errors.Select(s => new ErrorModel
			{
				Code = s.Code,
				Message = s.Description
			}));
		}

		return ResponseModel;
	}

	#region Private
	private string GenerateJwtToken(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(_authSettings.SecretKey);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[]
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
			}),
			Expires = DateTime.UtcNow.AddSeconds(_authSettings.Expires),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		var tokenString = tokenHandler.WriteToken(token);
		return tokenString;
	}

	#endregion
}

public interface IAuthService
{
	Task<ResponseModel> Login(LoginDto dto);
	Task<ResponseModel> Register(RegisterDto dto);
}