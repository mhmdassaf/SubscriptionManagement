namespace SubscriptionManagement.Common.Dtos.Auth;

public class LoginDto
{
	[Required, EmailAddress]
	public string? Email { get; set; }

	[DataType(DataType.Password)]
	[Required(ErrorMessage = $"{nameof(Password)} {Validation.IsRequired}")]
	[MinLength(8, ErrorMessage = $"{nameof(Password)} {Validation.MustBeAtLeast8Characters}")]
	public string? Password { get; set; }
}
