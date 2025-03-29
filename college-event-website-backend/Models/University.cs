namespace CollegeEvent.API.Models
{
	public class University
	{
		public int UniversityID { get; set; }

		public string Name { get; set; } = null!;

		public string Location { get; set; } = null!;

		public string? Description { get; set; }

		public int NumStudents { get; set; }
	}
}