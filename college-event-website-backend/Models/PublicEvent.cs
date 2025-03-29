namespace CollegeEvent.API.Models
{
	public class PublicEvent
	{
		public int EventID { get; set; }

		public int SuperAdminID { get; set; }

		public bool Approved { get; set; }
	}
}