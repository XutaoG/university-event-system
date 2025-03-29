namespace CollegeEvent.API.Dtos.Auth;

public class UserResponse
{
	public int UID { get; set; }

	public string Name { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string UserRole { get; set; } = null!;

	public int? UniversityID { get; set; }
}