namespace CollegeEvent.API.Models
{
	public class Location
	{
		public int LocID { get; set; }

		public string Name { get; set; } = null!;

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public string Address { get; set; } = null!;
	}
}