using System.ComponentModel.DataAnnotations;

namespace CollegeEvent.API.Dtos.Auth;

public class PasswordAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		if (value is string str && str.Contains(' '))
		{
			return new ValidationResult("Password cannot contain spaces");
		}

		return ValidationResult.Success;
	}
}

public class SignUpRequest
{
	[Required]
	[MinLength(3)]
	[MaxLength(16)]
	public string Name { get; set; } = null!;

	[Required]
	[MaxLength(100)]
	[EmailAddress]
	public string Email { get; set; } = null!;

	[Required]
	[MinLength(3)]
	[MaxLength(32)]
	[Password]
	public string Password { get; set; } = null!;

	[Required]
	public string UserRole { get; set; } = null!;
}