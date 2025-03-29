namespace CollegeEvent.API.Models;

public class User
{
	public int UID { get; set; }

	public string Name { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string PasswordHash { get; set; } = null!;

	public string UserRole { get; set; } = null!;

	public int? UniversityID { get; set; }
}
