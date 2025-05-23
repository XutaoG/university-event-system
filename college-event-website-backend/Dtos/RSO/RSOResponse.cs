namespace CollegeEvent.API.Dtos.RSO;

public class RSOResponse
{
	public int RSOID { get; set; }

	public string Name { get; set; } = null!;

	public string Description { get; set; } = null!;

	public int UniversityID { get; set; }

	public int AdminID { get; set; }

	public bool Active { get; set; }
}