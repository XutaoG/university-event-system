namespace CollegeEvent.API.Models
{
	public class RSO
	{
		public int RSOID { get; set; }

		public string Name { get; set; } = null!;

		public int UniversityID { get; set; }

		public int AdminID { get; set; }
	}
}