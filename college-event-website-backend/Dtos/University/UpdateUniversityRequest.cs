using System.ComponentModel.DataAnnotations;

namespace CollegeEvent.API.Dtos.University;

public class UpdateUniversityRequest
{
	[Required]
	public string Name { get; set; } = null!;

	[Required]
	[MaxLength(255)]
	public string Location { get; set; } = null!;

	public string? Description { get; set; }

	[Required]
	public int NumStudents { get; set; }
}