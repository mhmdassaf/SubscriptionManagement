using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SubscriptionManagement.Common.AppSettings;
using SubscriptionManagement.Common.Dtos.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ManageProducts.BAL.Services;

public class AuthService : BaseService, IAuthService
{
	private readonly AuthSettings _authSettings;

	public AuthService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
		IOptions<AuthSettings> options) : base(repository, mapper, httpContextAccessor)
	{
		_authSettings = options.Value;
	}

	public async Task<ResponseModel> Login(LoginDto dto)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(_authSettings.SecretKey);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[]
			{
				new Claim(ClaimTypes.Name, "username"),
				new Claim(ClaimTypes.Email, "user@example.com"),
			}),
			Expires = DateTime.UtcNow.AddSeconds(_authSettings.Expires),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		var tokenString = tokenHandler.WriteToken(token);
		ResponseModel.Result = tokenString;
		return ResponseModel;
	}

	public async Task<ResponseModel> Register(RegisterDto dto)
	{
		return ResponseModel;
	}
}

public interface IAuthService
{
	Task<ResponseModel> Login(LoginDto dto);
	Task<ResponseModel> Register(RegisterDto dto);
}