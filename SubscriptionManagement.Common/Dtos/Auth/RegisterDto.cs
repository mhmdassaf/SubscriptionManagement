namespace SubscriptionManagement.Common.Dtos.Auth;

public class RegisterDto
{
	[MinLength(3), MaxLength(10)]
	public string? FirstName { get; set; }

	[[MinLength(3), MaxLength(10)]
	public string? LastName { get; set; }

	[Required, EmailAddress]
	public string Email { get; set; } = null!;

	[DataType(DataType.Password)]
	[Required(ErrorMessage = $"{nameof(Password)} {Validation.IsRequired}")]
	[MinLength(8, ErrorMessage = $"{nameof(Password)} {Validation.MustBeAtLeast8Characters}")]
	public string Password { get; set; } = null!;

	[Compare(nameof(Password))]
	public string? ConfirmPassword { get; set; } = null!;
}
