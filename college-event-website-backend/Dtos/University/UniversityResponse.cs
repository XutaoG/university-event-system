namespace CollegeEvent.API.Dtos.University;

public class UniversityResponse
{
	public int UniversityID { get; set; }

	public string Name { get; set; } = null!;

	public string Location { get; set; } = null!;

	public string? Description { get; set; }

	public int NumStudents { get; set; }

	public string Domain { get; set; } = null!;
}