namespace CollegeEvent.API.Models
{
	public class Event
	{
		public int EventID { get; set; }

		public string Name { get; set; } = null!;

		public string EventCategory { get; set; } = null!;

		public string Description { get; set; } = null!;

		public DateTime EventTime { get; set; }

		public string ContactPhone { get; set; } = null!;

		public string ContactEmail { get; set; } = null!;

		public int LocID { get; set; }

		public int AdminID { get; set; }
	}
}